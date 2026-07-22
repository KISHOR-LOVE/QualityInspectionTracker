// ============================================================
//  QUALITY INSPECTION TRACKER  (Simple C# Console Project)
//  Concepts used: variables, if / else if / else, loops (while,
//  for, foreach), methods, method overloading, method overriding,
//  inheritance, classes and objects, List collection
// ============================================================

using System;
using System.Collections.Generic;
using System.IO;

namespace QualityInspectionTracker
{
    // ---------- CLASS (base class for INHERITANCE) ----------
    class Part
    {
        // VARIABLES (properties of the class)
        public string PartNumber = "";
        public string PartName = "";
        public string MachineNo = "";

        // METHOD (virtual = child class can OVERRIDE this)
        public virtual string Describe()
        {
            return PartNumber + " - " + PartName + " (Machine: " + MachineNo + ")";
        }
    }

    // ---------- INHERITANCE (EngineComponent inherits Part) ----------
    class EngineComponent : Part
    {
        public string ComponentType = "";   // extra variable only child has

        // METHOD OVERRIDING (same method name, new behaviour)
        public override string Describe()
        {
            return PartNumber + " - " + PartName + " [" + ComponentType + "] (Machine: " + MachineNo + ")";
        }
    }

    // ---------- CLASS (one inspection entry) ----------
    class InspectionRecord
    {
        public int Id;
        public string DateTimeText = "";
        public string Shift = "";
        public string InspectorName = "";
        public EngineComponent Component = new EngineComponent();  // OBJECT inside a class
        public string Result = "";      // "OK" or "NOTOK"
        public string Defect = "";      // "DIMENSION", "SURFACE", "CRACK", "THREAD", "OTHER", or "-"
        public string Remarks = "";
    }

    class Program
    {
        // VARIABLES (shared by all methods)
        static List<InspectionRecord> records = new List<InspectionRecord>();
        static int nextId = 1;

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("   QUALITY INSPECTION TRACKER  (C# Console)   ");
            Console.WriteLine("   Production parts OK / Not-OK management    ");
            Console.WriteLine("==============================================");

            bool running = true;

            // WHILE LOOP (menu keeps repeating until user exits)
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("1. New inspection entry");
                Console.WriteLine("2. View all records");
                Console.WriteLine("3. View rejected (Not-OK) parts");
                Console.WriteLine("4. Quality summary dashboard");
                Console.WriteLine("5. View records by shift");
                Console.WriteLine("6. Save report to text file");
                Console.WriteLine("0. Exit");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                // IF / ELSE IF / ELSE (deciding which option user selected)
                if (choice == "1")
                {
                    NewInspection();
                }
                else if (choice == "2")
                {
                    ShowRecords();                  // METHOD OVERLOADING - version 1 (no parameter)
                }
                else if (choice == "3")
                {
                    ShowRejected();
                }
                else if (choice == "4")
                {
                    ShowSummary();
                }
                else if (choice == "5")
                {
                    Console.Write("Enter shift (A/B/C): ");
                    string shift = Console.ReadLine().ToUpper();
                    ShowRecords(shift);             // METHOD OVERLOADING - version 2 (with parameter)
                }
                else if (choice == "6")
                {
                    SaveReport();
                }
                else if (choice == "0")
                {
                    running = false;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again.");
                }
            }

            Console.WriteLine("Goodbye!");
        }

        // ---------- METHOD: create new inspection ----------
        static void NewInspection()
        {
            // OBJECT creation from class
            EngineComponent component = new EngineComponent();
            component.PartNumber = Ask("Part number (e.g. CR-1042): ");
            component.PartName = Ask("Part name (e.g. Crankshaft): ");
            component.ComponentType = Ask("Component type (Crankshaft/Piston/Head): ");
            component.MachineNo = Ask("Machine no (e.g. CNC-07): ");

            string shift = Ask("Shift (A/B/C): ").ToUpper();
            string inspector = Ask("Inspector name: ");

            // Ask result until valid - WHILE LOOP + IF/ELSE
            string result = "";
            while (result == "")
            {
                string input = Ask("Result - OK or NOTOK: ").ToUpper();
                if (input == "OK")
                {
                    result = "OK";
                }
                else if (input == "NOTOK" || input == "NOT-OK" || input == "NG")
                {
                    result = "NOTOK";
                }
                else
                {
                    Console.WriteLine("Please type OK or NOTOK.");
                }
            }

            string defect = "-";
            string remarks = "";

            // Defect is asked only for rejected parts
            if (result == "NOTOK")
            {
                Console.WriteLine("Defect type:");
                Console.WriteLine("  1. DIMENSION (measurement out of tolerance)");
                Console.WriteLine("  2. SURFACE (scratch / dent / rust)");
                Console.WriteLine("  3. CRACK");
                Console.WriteLine("  4. THREAD (thread damage)");
                Console.WriteLine("  5. OTHER");
                string d = Ask("Choose defect number: ");

                // IF / ELSE IF ladder to convert number to defect name
                if (d == "1") { defect = "DIMENSION"; }
                else if (d == "2") { defect = "SURFACE"; }
                else if (d == "3") { defect = "CRACK"; }
                else if (d == "4") { defect = "THREAD"; }
                else { defect = "OTHER"; }

                remarks = Ask("Remarks (optional): ");
            }

            // OBJECT creation and filling values
            InspectionRecord record = new InspectionRecord();
            record.Id = nextId;
            nextId = nextId + 1;
            record.DateTimeText = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            record.Shift = shift;
            record.InspectorName = inspector;
            record.Component = component;
            record.Result = result;
            record.Defect = defect;
            record.Remarks = remarks;

            records.Add(record);   // adding object into List

            Console.WriteLine();
            if (result == "OK")
            {
                Console.WriteLine("[SAVED] #" + record.Id + " " + component.Describe() + " -> OK. Released to packing.");
            }
            else
            {
                Console.WriteLine("[SAVED] #" + record.Id + " " + component.Describe() + " -> NOT-OK (" + defect + "). Segregated from packing line.");
            }
        }

        // ---------- METHOD OVERLOADING version 1: show ALL records ----------
        static void ShowRecords()
        {
            if (records.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            PrintHeader();

            // FOREACH LOOP (going through every record in the List)
            foreach (InspectionRecord r in records)
            {
                PrintOneRecord(r);
            }
            Console.WriteLine("Total: " + records.Count + " record(s)");
        }

        // ---------- METHOD OVERLOADING version 2: show records of ONE shift ----------
        static void ShowRecords(string shift)
        {
            int count = 0;
            PrintHeader();

            // FOR LOOP (using index this time - to show both loop styles)
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].Shift == shift)
                {
                    PrintOneRecord(records[i]);
                    count = count + 1;
                }
            }

            if (count == 0)
            {
                Console.WriteLine("No records found for shift " + shift + ".");
            }
            else
            {
                Console.WriteLine("Total: " + count + " record(s) in shift " + shift);
            }
        }

        // ---------- METHOD: rejected parts only ----------
        static void ShowRejected()
        {
            Console.WriteLine("--- REJECTED PARTS (segregation list) ---");
            int count = 0;
            PrintHeader();

            foreach (InspectionRecord r in records)
            {
                if (r.Result == "NOTOK")
                {
                    PrintOneRecord(r);
                    count = count + 1;
                }
            }

            if (count == 0)
            {
                Console.WriteLine("No rejected parts. All OK!");
            }
            else
            {
                Console.WriteLine("Total rejected: " + count);
            }
        }

        // ---------- METHOD: summary dashboard (counting with loops) ----------
        static void ShowSummary()
        {
            if (records.Count == 0)
            {
                Console.WriteLine("No inspection records yet.");
                return;
            }

            // VARIABLES for counting
            int total = records.Count;
            int okCount = 0;
            int dimensionCount = 0;
            int surfaceCount = 0;
            int crackCount = 0;
            int threadCount = 0;
            int otherCount = 0;

            // FOREACH LOOP + IF/ELSE IF to count everything
            foreach (InspectionRecord r in records)
            {
                if (r.Result == "OK")
                {
                    okCount = okCount + 1;
                }
                else if (r.Defect == "DIMENSION") { dimensionCount = dimensionCount + 1; }
                else if (r.Defect == "SURFACE") { surfaceCount = surfaceCount + 1; }
                else if (r.Defect == "CRACK") { crackCount = crackCount + 1; }
                else if (r.Defect == "THREAD") { threadCount = threadCount + 1; }
                else { otherCount = otherCount + 1; }
            }

            int notOkCount = total - okCount;
            double passRate = (double)okCount / total * 100;

            Console.WriteLine("=========== QUALITY SUMMARY ===========");
            Console.WriteLine("Total inspected  : " + total);
            Console.WriteLine("OK (passed)      : " + okCount);
            Console.WriteLine("Not-OK (rejected): " + notOkCount);
            Console.WriteLine("Pass rate        : " + passRate.ToString("F1") + "%");
            Console.WriteLine();

            if (notOkCount > 0)
            {
                Console.WriteLine("Defect reasons:");
                // IF conditions - print only defects that happened
                if (dimensionCount > 0) { Console.WriteLine("  DIMENSION : " + dimensionCount); }
                if (surfaceCount > 0) { Console.WriteLine("  SURFACE   : " + surfaceCount); }
                if (crackCount > 0) { Console.WriteLine("  CRACK     : " + crackCount); }
                if (threadCount > 0) { Console.WriteLine("  THREAD    : " + threadCount); }
                if (otherCount > 0) { Console.WriteLine("  OTHER     : " + otherCount); }
            }
            Console.WriteLine("=======================================");
        }

        // ---------- METHOD: save simple text report ----------
        static void SaveReport()
        {
            if (records.Count == 0)
            {
                Console.WriteLine("No records to save.");
                return;
            }

            string report = "QUALITY INSPECTION REPORT\r\n";
            report = report + "Generated: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm") + "\r\n";
            report = report + "----------------------------------------\r\n";

            // Building the report line by line with a FOREACH LOOP
            foreach (InspectionRecord r in records)
            {
                report = report + "#" + r.Id + " | " + r.DateTimeText + " | Shift " + r.Shift
                       + " | " + r.Component.Describe() + " | " + r.Result;
                if (r.Result == "NOTOK")
                {
                    report = report + " | Defect: " + r.Defect;
                }
                report = report + " | " + r.InspectorName + "\r\n";
            }

            File.WriteAllText("report.txt", report);
            Console.WriteLine("[SAVED] Report written to report.txt");
        }

        // ---------- Small helper METHODS ----------
        static void PrintHeader()
        {
            Console.WriteLine("ID   Date/Time         Shift  Part                                     Result  Defect      Inspector");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------");
        }

        static void PrintOneRecord(InspectionRecord r)
        {
            Console.WriteLine("#" + r.Id + "   " + r.DateTimeText + "   " + r.Shift + "      "
                + r.Component.Describe() + "   " + r.Result + "   " + r.Defect + "   " + r.InspectorName);
        }

        static string Ask(string question)
        {
            Console.Write(question);
            string answer = Console.ReadLine();
            if (answer == null)
            {
                answer = "";
            }
            return answer.Trim();
        }
    }
}
