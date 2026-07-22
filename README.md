# Quality Inspection Tracker (C# Console Application)

A console application for managing production-line part inspections — recording OK / Not-OK decisions, listing rejected parts, and showing a quality summary report.

## Why I built this

Before moving into software, I worked one year in the Quality Department at Caparo Engineering India (an automotive engine parts manufacturer in Sunguvarchatram). My job was inspecting machined engine parts and deciding OK or Not-OK — only OK parts went to packing. That whole process was manual.

This project is the same workflow, built as software using C#.

## Features

- New inspection entry (part number, part name, component type, machine, shift, inspector)
- OK / Not-OK result — rejected parts need a defect reason (dimension, surface, crack, thread, other)
- Rejected parts list (segregation list — everything blocked from packing)
- Quality summary — total inspected, pass rate %, defect reason counts
- View records by shift (A / B / C)
- Save a full report to `report.txt`

## C# concepts used

| Concept | Where it is in the code |
|---|---|
| Class & Objects | `Part`, `EngineComponent`, `InspectionRecord` classes; objects created in `NewInspection()` |
| Inheritance | `EngineComponent : Part` |
| Method Overriding | `Describe()` — `virtual` in `Part`, `override` in `EngineComponent` |
| Method Overloading | `ShowRecords()` and `ShowRecords(string shift)` — same name, different parameters |
| Variables | `int`, `string`, `double`, `bool` used everywhere |
| if / else if / else | Menu selection, defect selection, result validation |
| while loop | Main menu loop, result input validation |
| for loop | Shift-wise record filtering |
| foreach loop | Printing records, counting for summary, building report text |
| List collection | `List<InspectionRecord>` stores all records |
| Methods | `NewInspection()`, `ShowSummary()`, `SaveReport()`, `Ask()` and more |

## How to run

Requires [.NET SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run
```

## Sample output

```
=========== QUALITY SUMMARY ===========
Total inspected  : 3
OK (passed)      : 1
Not-OK (rejected): 2
Pass rate        : 33.3%

Defect reasons:
  DIMENSION : 1
  SURFACE   : 1
=======================================
```

## Author

Kishorkumar K — [LinkedIn](https://linkedin.com/in/kishor-kumar-mba) · [Portfolio](https://kishorportfoliomba.netlify.app)
