using System;
using System.IO;

namespace meet_the_family
{
    class Program
    {
        static FamilyTree familyTree;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Provide input file argument");
                return;
            }

            // string filePath = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            string filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found");
                return;
            }

            familyTree = new FamilyTree();
            familyTree.Initialize();

            var lines = File.ReadLines(filePath);
            foreach (var line in lines)
            {
                string[] parts = line.Split(' ');
                string commandString = parts[0];
                Command command = GetCommandEnum(commandString);
                ProcessCommand(command, parts);
            }
        }

        private static void ProcessCommand(Command command, string[] parts)
        {
            string result;
            switch (command)
            {
                case Command.ADD_CHILD:
                    string mother = parts[1];
                    string child = parts[2];
                    Gender gender = GenderToEnum(parts[3]);
                    result = familyTree.AddChild(mother, child, gender);
                    Console.WriteLine(result);
                    break;

                case Command.GET_RELATIONSHIP:
                    string person = parts[1];
                    Relation relation = RelationToEnum(parts[2]);
                    result = familyTree.GetRelationship(person, relation);
                    Console.WriteLine(result);
                    break;
            }
        }

        private static Command GetCommandEnum(string commandString)
        {
            switch (commandString)
            {
                case "ADD_CHILD": return Command.ADD_CHILD;
                case "GET_RELATIONSHIP": return Command.GET_RELATIONSHIP;
                default: return Command.NONE;
            }
        }

        private static Gender GenderToEnum(string genderString)
        {
            switch (genderString)
            {
                case "Male": return Gender.MALE;
                case "Female": return Gender.FEMALE;
                default: return Gender.NONE;
            }
        }

        private static Relation RelationToEnum(string relation)
        {
            switch (relation)
            {
                case "Paternal-Uncle": return Relation.PATERNAL_UNCLE;
                case "Maternal-Uncle": return Relation.MATERNAL_UNCLE;
                case "Paternal-Aunt": return Relation.PATERNAL_AUNT;
                case "Maternal-Aunt": return Relation.MATERNAL_AUNT;
                case "Sister-In-Law": return Relation.SISTER_IN_LAW;
                case "Brother-In-Law": return Relation.BROTHER_IN_LAW;
                case "Son": return Relation.SON;
                case "Daughter": return Relation.DAUGHTER;
                case "Siblings": return Relation.SIBLINGS;
                default: return Relation.NONE;
            }
        }
    }
}
