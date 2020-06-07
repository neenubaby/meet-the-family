using System.Collections.Generic;
using System.Linq;

namespace meet_the_family
{
    class FamilyTree
    {
        public Node root;

        public void Initialize()
        {
            Node king = CreateMember("King Shan", Gender.MALE);
            Node queen = CreateMember("Queen Anga", Gender.FEMALE);

            root = king;
            Married(king, queen);

            Node chit = CreateMember("Chit", Gender.MALE);
            AddChild(queen, chit);

            Node amba = CreateMember("Amba", Gender.FEMALE);
            Married(chit, amba);

            Node ish = CreateMember("Ish", Gender.MALE);
            AddChild(queen, ish);

            Node vich = CreateMember("Vich", Gender.MALE);
            AddChild(queen, vich);

            Node lika = CreateMember("Lika", Gender.FEMALE);
            Married(vich, lika);

            Node aras = CreateMember("Aras", Gender.MALE);
            AddChild(queen, aras);

            Node chitra = CreateMember("Chitra", Gender.FEMALE);
            Married(aras, chitra);

            Node satya = CreateMember("Satya", Gender.FEMALE);
            AddChild(queen, satya);

            Node vyan = CreateMember("Vyan", Gender.MALE);
            Married(vyan, satya);

            ///
            Node dritha = CreateMember("Dritha", Gender.FEMALE);
            AddChild(amba, dritha);

            Node jaya = CreateMember("Jaya", Gender.MALE);
            Married(jaya, dritha);

            Node tritha = CreateMember("Tritha", Gender.FEMALE);
            AddChild(amba, tritha);

            Node vritha = CreateMember("Vritha", Gender.MALE);
            AddChild(amba, vritha);

            Node vila = CreateMember("Vila", Gender.FEMALE);
            Node chika = CreateMember("Chika", Gender.FEMALE);
            AddChild(lika, vila);
            AddChild(lika, chika);

            Node jnki = CreateMember("Jnki", Gender.FEMALE);
            Node ahit = CreateMember("Ahit", Gender.MALE);
            AddChild(chitra, jnki);
            AddChild(chitra, ahit);
            Node arit = CreateMember("Arit", Gender.MALE);
            Married(arit, jnki);

            Node asva = CreateMember("Asva", Gender.MALE);
            Node vyas = CreateMember("Vyas", Gender.MALE);
            Node atya = CreateMember("Atya", Gender.FEMALE);
            AddChild(satya, asva);
            AddChild(satya, vyas);
            AddChild(satya, atya);
            Node satvy = CreateMember("Satvy", Gender.FEMALE);
            Node krpi = CreateMember("Krpi", Gender.FEMALE);
            Married(asva, satvy);
            Married(vyas, krpi);

            ///

            Node yodhan = CreateMember("Yodhan", Gender.MALE);
            AddChild(dritha, yodhan);

            Node laki = CreateMember("Laki", Gender.MALE);
            Node lavanya = CreateMember("Lavanya", Gender.FEMALE);
            AddChild(jnki, laki);
            AddChild(jnki, lavanya);

            Node vasa = CreateMember("Vasa", Gender.MALE);
            AddChild(satvy, vasa);

            Node kriya = CreateMember("Kriya", Gender.MALE);
            AddChild(krpi, kriya);
            Node krithi = CreateMember("Krithi", Gender.FEMALE);
            AddChild(krpi, krithi);
        }

        public Node CreateMember(string name, Gender gender)
        {
            Node node = new Node();
            node.Name = name;
            node.Gender = gender;
            return node;
        }

        public void Married(Node husband, Node wife)
        {
            husband.Spouse = wife;
            wife.Spouse = husband;
        }

        public void AddChild(Node mother, Node child)
        {
            mother.Children.Add(child);
            mother.Spouse.Children.Add(child);
            child.Parents.Add(Parent.MOTHER, mother);
            child.Parents.Add(Parent.FATHER, mother.Spouse);
        }

        public string AddChild(string motherName, string childName, Gender gender)
        {
            Node mother = Find(motherName);

            if (mother == null)
                return "PERSON_NOT_FOUND";

            if (mother.Gender != Gender.FEMALE)
                return "CHILD_ADDITION_FAILED";

            Node child = new Node();
            child.Name = childName;
            child.Gender = gender;

            mother.Children.Add(child);
            mother.Spouse.Children.Add(child);
            child.Parents.Add(Parent.MOTHER, mother);
            child.Parents.Add(Parent.FATHER, mother.Spouse);

            return "CHILD_ADDITION_SUCCEEDED";
        }

        public string GetRelationship(string memberName, Relation relation)
        {
            Node member = Find(memberName);

            if (member == null)
                return "PERSON_NOT_FOUND";

            List<Node> relatives = GetRelatives(member, relation);

            if (relatives == null || relatives.Count == 0)
                return "NONE";

            return string.Join(" ", relatives.Select(relative => relative.Name));
        }

        private List<Node> GetRelatives(Node member, Relation relation)
        {
            List<Node> relativeList = new List<Node>();
            switch (relation)
            {
                case Relation.PATERNAL_UNCLE:
                    if (member.Parents.ContainsKey(Parent.FATHER))
                    {
                        Node father = member.Parents[Parent.FATHER];
                        relativeList.AddRange(GetBrothers(father));
                    }
                    break;

                case Relation.MATERNAL_UNCLE:
                    if (member.Parents.ContainsKey(Parent.MOTHER))
                    {
                        Node mother = member.Parents[Parent.MOTHER];
                        relativeList.AddRange(GetBrothers(mother));
                    }
                    break;

                case Relation.PATERNAL_AUNT:
                    if (member.Parents.ContainsKey(Parent.FATHER))
                    {
                        Node father = member.Parents[Parent.FATHER];
                        relativeList.AddRange(GetSisters(father));
                    }
                    break;

                case Relation.MATERNAL_AUNT:
                    if (member.Parents.ContainsKey(Parent.MOTHER))
                    {
                        Node mother = member.Parents[Parent.MOTHER];
                        relativeList.AddRange(GetSisters(mother));
                    }
                    break;

                case Relation.SISTER_IN_LAW:
                    if (member.Spouse != null)
                        relativeList.AddRange(GetSisters(member.Spouse));

                    List<Node> brothers = GetBrothers(member);
                    foreach (Node brother in brothers)
                    {
                        if (brother.Spouse != null)
                            relativeList.Add(brother.Spouse);
                    }
                    break;

                case Relation.BROTHER_IN_LAW:
                    if (member.Spouse != null)
                        relativeList.AddRange(GetBrothers(member.Spouse));

                    List<Node> sisters = GetSisters(member);
                    foreach (Node sister in sisters)
                    {
                        if (sister.Spouse != null)
                            relativeList.Add(sister.Spouse);
                    }
                    break;

                case Relation.SON:
                    relativeList.AddRange(member.Children.Where(child => child.Gender == Gender.MALE));
                    break;

                case Relation.DAUGHTER:
                    relativeList.AddRange(member.Children.Where(child => child.Gender == Gender.FEMALE));
                    break;

                case Relation.SIBLINGS:
                    relativeList.AddRange(GetSiblings(member));
                    break;

                default:
                    break;
            }
            return relativeList;
        }

        private List<Node> GetBrothers(Node member)
        {
            return GetSiblings(member).Where(child => child.Gender == Gender.MALE).ToList();
        }

        private List<Node> GetSisters(Node member)
        {
            return GetSiblings(member).Where(child => child.Gender == Gender.FEMALE).ToList();
        }

        private List<Node> GetSiblings(Node member)
        {
            List<Node> siblings = new List<Node>();
            if (member.Parents.ContainsKey(Parent.MOTHER))
            {
                Node mother = member.Parents[Parent.MOTHER];
                siblings.AddRange(mother.Children.Where(child => child.Name != member.Name));
            }
            return siblings;
        }

        public Node Find(string nameToSearchFor)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> nodesCovered = new HashSet<Node>();
            queue.Enqueue(root);
            nodesCovered.Add(root);

            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                if (node.Name == nameToSearchFor)
                    return node;
                if (node.Spouse?.Name == nameToSearchFor)
                    return node.Spouse;
                foreach (Node child in node.Children)
                {
                    if (!nodesCovered.Contains(child))
                    {
                        queue.Enqueue(child);
                        nodesCovered.Add(child);
                    }
                }
            }
            return null;
        }
    }
}
