using System;
using StudentExercise5.Data;

namespace StudentExercise5
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new Repository();

            var allExercises = repo.GetAllExercise();

            // Made a new exercise //
            //repo.addNewExercise("Diggers&Fliers", "C#");

            //Made a new Instructor //
            //repo.addInstructor("Nick", "Wessel", "wesseln1", "Jokes", 1);

            Console.WriteLine("All exercises");
            Console.WriteLine("--------------------------");

            foreach (var exercise in allExercises)
            {
                Console.WriteLine($"Id {exercise.Id}| {exercise.Name}: {exercise.Language}");
            }

            var allJavaScriptExercises = repo.GetAllJavascript();
            Console.WriteLine();
            Console.WriteLine("All Java Script exercises");
            Console.WriteLine("--------------------------");

            foreach (var exercise in allJavaScriptExercises)
            {
                Console.WriteLine($"Id: {exercise.Id}| {exercise.Name}: {exercise.Language}");
            }

            Console.WriteLine();
            Console.WriteLine("All Instructors");
            Console.WriteLine("--------------------------");

            var allInstructors = repo.getAllInstructors();

            foreach (var instructor in allInstructors)
            {
                Console.WriteLine($"Id: {instructor.Id}");
                Console.WriteLine($"Name:{instructor.FirstName} {instructor.LastName}");
                Console.WriteLine($"Specialty: {instructor.Speciality}");
                Console.WriteLine($"Slack Handle: {instructor.SlackHandle}");
                Console.WriteLine($"Class: {instructor.CohortName}");

                Console.WriteLine();
            }

            Console.WriteLine("Would you like to assign an exercise to a student?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            var option = Console.ReadLine();

            if(option == "1")
            {
                Console.Clear();

                var allStudents = repo.getStudents();

                foreach (var student in allStudents)
                {
                    Console.WriteLine($"Id: {student.Id}");
                    Console.WriteLine($"First Name: {student.FirstName}");
                    Console.WriteLine($"Last Name: {student.LastName}");
                    Console.WriteLine();
                }
                Console.WriteLine("Which student would you like to assign to? (Choose by Id)");
                var studentId = Int32.Parse(Console.ReadLine());
                Console.Clear();
                var allExercisesToChoose = repo.GetAllExercise();

                foreach (var exercise in allExercisesToChoose)
                {
                    Console.WriteLine($"Id: {exercise.Id}");
                    Console.WriteLine($"Name: {exercise.Name}");
                    Console.WriteLine($"Language: {exercise.Language}");
                    Console.WriteLine();
                }

                Console.WriteLine("Which exercise would you like to give this student? (Choose by Id)");
                var exerciseId = Int32.Parse(Console.ReadLine());

                repo.assignExercise(studentId, exerciseId);

                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"Great! You've succesfully assigned the exercise too the student with the id of {studentId}!");
            }

            
        }
    }
}
