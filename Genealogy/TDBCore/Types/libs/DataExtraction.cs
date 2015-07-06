using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TDBCore.Types.libs
{
    public class DataExtraction
    {
        public static string MakePercentage(int total, int current)
        {
            decimal percentage = decimal.Zero;

            decimal onepercent = Convert.ToDecimal( 100)/ total ;

            percentage = onepercent * current;


            return percentage.ToString("N4");
        }

        private void ExtractDeaths(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));

            var query = from line in _allLines
                        where line.ToCharArray().Where(o => o == '|').Count() == 24

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Burial of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace("[Chapel/Registry]", ""),
                                Full_Name = data[4].Replace("Full Name", ""),
                                Date_of_Death = data[5].Replace("Date of Death", ""),
                                Place_of_Death = data[6].Replace("Place of Death", ""),
                                Date_of_Birth = data[7].Replace("Date of Birth", ""),

                                Age = data[9].Replace("Age", ""),
                                Profession = data[10].Replace("Profession", ""),
                                Relation = data[11].Replace("Relation", ""),
                                Description = data[12].Replace("Description", ""),
                                Place_of_Abode = data[13].Replace("Place of Abode", ""),
                                Parish_of_Abode = data[14].Replace("Parish of Abode", ""),
                                County_of_Abode = data[15].Replace("County of Abode", ""),
                                Registration_Date = data[16].Replace("Registration Date", ""),
                                Registration_Town_County = data[17].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[18].Replace("Ceremony Performed by", ""),
                                Husband_Father = data[19].Replace("Husband's/Father's", ""),
                                Husbands_Fathers_Profession = data[20].Replace("Husband's/Father's Profession", ""),
                                Wife_Mother = data[21].Replace("Wife/Mother -", ""),
                                Cause_of_death = data[22].Replace("Cause of death", ""),
                                Grave_Number = data[23].Replace("Grave Number", ""),
                                Undertaker = data[24].Replace("Undertaker", ""),

                            };




            int idx = 0;
            foreach (var team in query)
            {
                //  if (idx == 89)
                Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }
        }


        private void ExtractBurials(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));


            int idx = 0;
            foreach (string str in _allLines)
            {
                //if(str.Select(m=> { str.

                if (str.ToCharArray().Where(o => o == '|').Count() != 25)
                {
                    Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
                    Debug.WriteLine(str);

                }


                idx++;
            }



            var query = from line in _allLines

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Burial of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace("[Chapel/Registry]", ""),
                                Full_Name = data[4].Replace("Full Name", ""),
                                Date_of_Burial = data[5].Replace("Date of Burial", ""),
                                Place_of_Burial = data[6].Replace("Place of Burial", ""),
                                Date_of_Birth = data[7].Replace("Date of Birth", ""),
                                Date_of_Death = data[8].Replace("Date of Death", ""),
                                Age = data[9].Replace("Age", ""),
                                Profession = data[10].Replace("Profession", ""),
                                Relation = data[11].Replace("Relation", ""),
                                Description = data[12].Replace("Description", ""),
                                Place_of_Abode = data[13].Replace("Place of Abode", ""),
                                Parish_of_Abode = data[14].Replace("Parish of Abode", ""),
                                County_of_Abode = data[15].Replace("County of Abode", ""),
                                Registration_Date = data[16].Replace("Registration Date", ""),
                                Registration_Town_County = data[17].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[18].Replace("Ceremony Performed by", ""),
                                Husband_Father = data[19].Replace("Husband's/Father's", ""),
                                Husbands_Fathers_Profession = data[20].Replace("Husband's/Father's Profession", ""),
                                Wife_Mother = data[21].Replace("Wife/Mother -", ""),
                                Cause_of_death = data[22].Replace("Cause of death", ""),
                                Grave_Number = data[23].Replace("Grave Number", ""),
                                Undertaker = data[24].Replace("Undertaker", ""),

                            };




            idx = 0;
            foreach (var team in query)
            {
                if (idx == 89)
                    Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }
        }


        private void ExtractBirths(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));

            //foreach (string _line in _allLines)
            //{
            //   // Debug.WriteLine(_line.Replace("|", "|\r\n"));
            //    string newLine = _line.Substring(0, _line.LastIndexOf("Grandparent(s)|") + 15);
            //    if (newLine.ToCharArray().Where(o => o == '|').Count() != 31)
            //        Debug.WriteLine("  " +  newLine);
            //    else
            //        Debug.WriteLine(newLine);


            //}

            //int idx = 0;

            var query = from line in _allLines
                        where line.ToCharArray().Where(o => o == '|').Count() == 31

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Baptism of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace(@"[Chapel/Registry]", ""),
                                Full_Name = data[4].Replace("Full Name", ""),

                                Date_of_Birth = data[5].Replace("Date of Birth", ""),
                                Place_of_Birth = data[6].Replace("Place of Birth", ""),

                                Place_of_Abode = data[8].Replace("Place of Abode", ""),
                                Parish_of_Abode = data[9].Replace("Parish of Abode", ""),
                                County_of_Abode = data[10].Replace("County of Abode", ""),
                                Registration_Date = data[11].Replace("Registration Date", ""),
                                Registration_Town_County = data[12].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[13].Replace("Ceremony Performed by", ""),



                                //    Godparents = data[14].Replace("Godparents", ""),
                                //    Godfather = data[15].Replace("Godfather", ""),
                                //    Godmother = data[16].Replace("Godmother", ""),
                                Parents = data[14].Replace("Parents", ""),
                                Father = data[15].Replace("Father", ""),
                                Fathers_Profession = data[16].Replace("Father's Profession", ""),
                                Mother = data[17].Replace("Mother", ""),
                                Mothers_Maiden_Name = data[18].Replace("Mother's Maiden Name", ""),
                                Mothers_Parish = data[19].Replace("Mother's Parish", ""),
                                Date_of_Marriage = data[20].Replace("Date of Marriage", ""),
                                Place_of_Marriage = data[21].Replace("Place of Marriage", ""),
                                Maternal_Parents = data[22].Replace("Maternal Parents", ""),
                                Names = data[23].Replace("Name(s)", ""),
                                Profession = data[24].Replace("Profession", ""),
                                TownCounty = data[25].Replace("Town & County", ""),
                                PaternalParents = data[26].Replace("Paternal Parents", ""),
                                Namesx2 = data[27].Replace("Name(s)", ""),
                                Professionx2 = data[28].Replace("Profession", ""),
                                PedigreeChart = data[29].Replace("Pedigree Chart", ""),
                                Grandparentsx1 = data[30].Replace("Grandparent(s)", ""),
                                Grandparentsx2 = data[31].Replace("Grandparent(s)", ""),





                            };




            int idx = 0;
            foreach (var team in query)
            {

                Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }
        }


        private void ExtractBaptisms(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));


            //int idx = 0;
            //foreach (string str in _allLines)
            //{
            //    //if(str.Select(m=> { str.

            //    if (str.ToCharArray().Where(o => o == '|').Count() == 39)
            //    {
            //   //     Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
            //        Debug.WriteLine(str + "|||");

            //    }


            //    idx++;
            //}

            // idx = 0;
            //foreach (string str in _allLines)
            //{
            //    //if(str.Select(m=> { str.

            //       if (str.ToCharArray().Where(o => o == '|').Count() == 40)
            //       {
            //  //  Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
            //           Debug.WriteLine(str + "||");

            //       }


            //    idx++;
            //}

            // idx = 0;
            //foreach (string str in _allLines)
            //{
            //    //if(str.Select(m=> { str.

            //       if (str.ToCharArray().Where(o => o == '|').Count() == 41)
            //       {
            //  //  Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
            //           Debug.WriteLine(str + "|");
            //       }


            //    idx++;
            //}


            //idx = 0;
            //foreach (string str in _allLines)
            //{
            //    //if(str.Select(m=> { str.

            //       if (str.ToCharArray().Where(o => o == '|').Count() == 42)
            //       {
            //   // Debug.WriteLine("record: " + idx + " contains " + str.ToCharArray().Where(o => o == '|').Count().ToString());
            //           Debug.WriteLine(str);

            //       }


            //    idx++;
            //}










            var query = from line in _allLines

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Baptism of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace(@"[Chapel/Registry]", ""),
                                Full_Name = data[4].Replace("Full Name", ""),

                                Date_of_Baptism = data[5].Replace("Date of Baptism", ""),
                                Place_of_Baptism = data[6].Replace("Place of Baptism", ""),
                                Date_of_Birth = data[7].Replace("Date of Birth", ""),
                                Place_of_Abode = data[8].Replace("Place of Abode", ""),
                                Parish_of_Abode = data[9].Replace("Parish of Abode", ""),
                                County_of_Abode = data[10].Replace("County of Abode", ""),
                                Registration_Date = data[11].Replace("Registration Date", ""),
                                Registration_Town_County = data[12].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[13].Replace("Ceremony Performed by", ""),



                                Godparents = data[14].Replace("Godparents", ""),
                                Godfather = data[15].Replace("Godfather", ""),
                                Godmother = data[16].Replace("Godmother", ""),
                                Parents = data[17].Replace("Parents", ""),
                                Father = data[18].Replace("Father", ""),
                                Fathers_Profession = data[19].Replace("Father's Profession", ""),
                                Mother = data[20].Replace("Mother", ""),
                                Mothers_Maiden_Name = data[21].Replace("Mother's Maiden Name", ""),
                                Mothers_Parish = data[22].Replace("Mother's Parish", ""),
                                Date_of_Marriage = data[23].Replace("Date of Marriage", ""),
                                Place_of_Marriage = data[24].Replace("Place of Marriage", ""),
                                Maternal_Parents = data[25].Replace("Maternal Parents", ""),
                                Names = data[26].Replace("Name(s)", ""),
                                Profession = data[27].Replace("Profession", ""),
                                TownCounty = data[28].Replace("Town & County", ""),
                                PaternalParents = data[29].Replace("Paternal Parents", ""),
                                Namesx2 = data[30].Replace("Name(s)", ""),
                                Professionx2 = data[31].Replace("Profession", ""),
                                PedigreeChart = data[32].Replace("Pedigree Chart", ""),
                                Grandparentsx1 = data[33].Replace("Grandparent(s)", ""),
                                Grandparentsx2 = data[34].Replace("Grandparent(s)", ""),
                                Notes = data[35] + data[36] + data[37] + data[38] + data[39] + data[40] + data[41] + data[42]





                            };




            int idx = 0;
            foreach (var team in query)
            {

                Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }
        }

        private void ExtractMarriages(string path)
        {
            List<string> _allLines = new List<string>(File.ReadAllLines(path));






            var query = from line in _allLines

                        let data = line.Split('|')

                        select new

                            {
                                Name = data[0].Replace("Baptism of", ""),
                                Piece_Description = data[1].Replace("PD", ""),
                                TNA_Reference = data[2].Replace("TNA Reference", ""),
                                Chapel = data[3].Replace(@"[Chapel/Registry]", ""),
                                Groom_Name = data[4].Replace("Groom Name", ""),
                                Bride_Name = data[5].Replace("Bride Name", ""),
                                Grooms_Profession = data[6].Replace("Groom's Profession", ""),
                                Date_of_Marriage = data[7].Replace("Date of Marriage", ""),
                                Place_of_Marriage = data[8].Replace("Place of Marriage", ""),
                                Registration_Date = data[9].Replace("Registration Date", ""),
                                Registration_Town_County = data[10].Replace("Registration Town/County", ""),
                                Ceremony_Performed_by = data[11].Replace("Ceremony Performed by", ""),
                                Grooms_Abode = data[12].Replace("Groom's Abode", ""),
                                Brides_Abode = data[13].Replace("Bride's Abode", ""),
                                Grooms_Parents = data[14].Replace("Groom's Parents", ""),
                                Grooms_Father = data[15].Replace("Groom's Father", ""),
                                Grooms_Fathers = data[16].Replace("Groom's Father's", ""),
                                Fathers_Profession = data[17].Replace("Father's Profession", ""),
                                Grooms_Fathers_Abode = data[18].Replace("Groom's Father's Abode", ""),
                                Grooms_Mother = data[19].Replace("Groom's Mother", ""),
                                Grooms_Mothers_Abode = data[20].Replace("Groom's Mother's Abode", ""),
                                Brides_Parents = data[21].Replace("Bride's Parents", ""),


                                Brides_Father = data[22].Replace("Bride's Father", ""),
                                Brides_Fathers_Profession = data[23].Replace("Bride's Father's Profession", ""),
                                Brides_Fathers_Abode = data[24].Replace("Bride's Father's Abode", ""),
                                Brides_Mother = data[25].Replace("Bride's Mother", ""),



                                Brides_Mothers_Adobe = data[26].Replace("Bride's Mother's Adobe", ""),
                                Pedigree_Chart = data[27].Replace("Pedigree Chart", ""),






                            };




            int idx = 0;
            foreach (var team in query)
            {

                Debug.WriteLine(team.TNA_Reference + "," + team.Piece_Description);


                idx++;
            }



        }

    }
}