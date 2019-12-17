using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using StudentExercise5.Models;

namespace StudentExercise5.Data
{
    class Repository
    {
        public SqlConnection Connection
        {
            get
            {
                // This is "address" of the database
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=StudentExercise;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }
        public List<Exercise> GetAllExercise()
        {
            //  We must "use" the database connection.
            //  Because a database is a shared resource (other applications may be using it too) we must
            //  be careful about how we interact with it. Specifically, we Open() connections when we need to
            //  interact with the database and we Close() them when we're finished.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block   doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, Name, Language FROM Exercise";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the departments we retrieve from the database.
                    List<Exercise> exercises = new List<Exercise>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "DeptName" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int exerciseNameColumnPosition = reader.GetOrdinal("Name");
                        string exerciseNameValue = reader.GetString(exerciseNameColumnPosition);

                        int exerciseLanguageColumnPosition = reader.GetOrdinal("Language");
                        string exerciseLanguageValue = reader.GetString(exerciseLanguageColumnPosition);

                        // Now let's create a new department object using the data from the database.
                        Exercise exercise = new Exercise
                        {
                            Id = idValue,
                            Name = exerciseNameValue,
                            Language = exerciseLanguageValue
                        };

                        // ...and add that department object to our list.
                        exercises.Add(exercise);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of departments who whomever called this method.
                    return exercises;
                }
            }
        }
        /// <summary>
        ///  Returns a single department with the given id.
        /// </summary>
        /// 
        public List<Exercise> GetAllJavascript()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Language From Exercise WHERE Language = 'Javascript'";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> javaScriptExercises = new List<Exercise>();

                    while(reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int exerciseNameColumnPosistion = reader.GetOrdinal("Name");
                        string exerciseNameValue = reader.GetString(exerciseNameColumnPosistion);

                        int exerciseLanguageColumnPosistion = reader.GetOrdinal("Language");
                        string exerciseLanguageValue = reader.GetString(exerciseLanguageColumnPosistion);

                        Exercise exercise = new Exercise()
                        {
                            Id = idValue,
                            Name = exerciseNameValue,
                            Language = exerciseLanguageValue
                        };

                        javaScriptExercises.Add(exercise);
                    }
                    reader.Close();
                    return javaScriptExercises;
                }
            }
        }

        public void addNewExercise(string name, string language)
        {
            var newExercise = new Exercise() { Name = name, Language = language };
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Exercise (Name, Language) OUTPUT INSERTED.Id Values (@name, @language)";
                    cmd.Parameters.Add(new SqlParameter("@name", newExercise.Name));
                    cmd.Parameters.Add(new SqlParameter("@language", newExercise.Language));
                    int id = (int)cmd.ExecuteScalar();

                    newExercise.Id = id;
                }
            }
        }

        public List<Instructor> getAllInstructors()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Instructor.Id, FirstName, LastName, SlackHandle, Speciality, Cohort.Name FROM Instructor LEFT JOIN Cohort on Instructor.CohortId = Cohort.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> insturctors = new List<Instructor>();


                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int instructorFirstNameColumnPosistion = reader.GetOrdinal("FirstName");
                        string instructorFirstNameValue = reader.GetString(instructorFirstNameColumnPosistion);

                        int instructorLastNameColumnPosistion = reader.GetOrdinal("LastName");
                        string instructorLastNameValue = reader.GetString(instructorLastNameColumnPosistion);

                        int instructorSlackHandleColumnPosistion = reader.GetOrdinal("SlackHandle");
                        string instructorSlackHandleValue = reader.GetString(instructorSlackHandleColumnPosistion);

                        int instructorSpecialityColumnPosistion = reader.GetOrdinal("Speciality");
                        string instructorSpecialityValue = reader.GetString(instructorSpecialityColumnPosistion);

                        int cohortNameColumnPosistion = reader.GetOrdinal("Name");
                        string cohortNameValue = reader.GetString(cohortNameColumnPosistion);

                        Instructor newInstructor = new Instructor()
                        {
                            Id = idValue,
                            FirstName = instructorFirstNameValue,
                            LastName = instructorLastNameValue,
                            Speciality = instructorSpecialityValue,
                            SlackHandle = instructorSlackHandleValue,
                            CohortName = cohortNameValue
                        };
                        insturctors.Add(newInstructor);
                    }
                        reader.Close();
                        return insturctors;
                }
            }
        }

        public void addInstructor(string firstName, string lastName, string slackHandle, string speciality, int cohortId)
        {
            var newInstructor = new Instructor() { FirstName = firstName, LastName = lastName, SlackHandle = slackHandle, Speciality = speciality, CohortId = cohortId};
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Student.Id, FirstName, LastName, SlackHandle, Cohort.Name FROM Instructor LEFT JOIN Cohort on Instructor.CohortId = Cohort.Id";

                    cmd.CommandText = "INSERT INTO Instructor (FirstName, LastName, SlackHandle, Speciality, CohortId) OUTPUT INSERTED.Id Values (@firstName, @lastName, @slackHandle, @speciality, @cohortId)";
                    cmd.Parameters.Add(new SqlParameter("@firstName", newInstructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", newInstructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackHandle", newInstructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@speciality", newInstructor.Speciality));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", newInstructor.CohortId));
                    int id = (int)cmd.ExecuteScalar();

                    newInstructor.Id = id;
                }
            }
        }
        public void assignExercise(int studentId, int exerciseId)
        {
            var newAssignment = new StudentExercise() { StudentId = studentId, ExerciseId = exerciseId };
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO StudentExercise (StudentId, ExerciseId) OUTPUT INSERTED.Id Values (@studentId, @exerciseId)";
                    cmd.Parameters.Add(new SqlParameter("@studentId", newAssignment.StudentId));
                    cmd.Parameters.Add(new SqlParameter("@exerciseId", newAssignment.ExerciseId));
                    int id = (int)cmd.ExecuteScalar();

                    newAssignment.Id = id;
                }
            }
        }

        public List<Student> getStudents()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName FROM Student";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> allStudents = new List<Student>();

                    while(reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosistion = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosistion);

                        int lastNameColumnPosistion = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosistion);

                        var student = new Student()
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue
                        };

                        allStudents.Add(student);
                    }
                    reader.Close();

                    return allStudents;
                }
            }
        }
    }
}
