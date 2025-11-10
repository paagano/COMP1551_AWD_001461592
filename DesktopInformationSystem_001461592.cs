// DesktopInformationSystem_001461592.cs
// C# Desktop Application for Education Centre Management Information System
// Developed By: Philip Agano - 001461592
// This application manages Teachers, Admins, and Students records
// Demonstrates OOP concepts: Inheritance, Encapsulation, Polymorphism
// Uses a console-based menu for user interaction
// Data is persisted in a CSV file using StreamReader and StreamWriter classes
// No external libraries or databases are used

using System; // Imports the core system namespace for basic functionalities
using System.Collections.Generic; // Imports the generic collections namespace so I can use List<T>
using System.IO; // For file Input/Output operations (StreamReader and StreamWriter)

// =============================
//  BASE CLASS: Person
// =============================

// Abstract base class representing a generic person i.e cannot create new Person() directly. it's intended to be inherited.
// Encapsulates common fields and methods for derived classes
// Implements polymorphism via virtual methods
// Inheritance is used to create specific person types (Teacher, Admin, Student)
public abstract class Person
{
    // Private fields (Encapsulated i.e can't be accessed directly from outside the Person class.)
    // Common fields for all derived classes (Id, Name, Telephone, Email, Role)
    // Id is unique identifier, set only once in constructor
    private int id;
    private string name;
    private string telephone;
    private string email;
    private string role;

    // Public properties (provides controlled access to the private fields id, name, telephone, email, role)
    // Id is read-only outside the class, set only in constructor. 
    public int Id
    {
        // Read-only outside the class
        // The setter is protected, meaning only this class (Person) and its subclasses (derived classes) can assign a new value to Id.
        // Id is set only once during construction
        get => id;
        protected set => id = value; // controlled encapsulation pattern
    }

    // Name property with basic validation (using ternary conditional operator)
    // Trims whitespace and ensures non-empty i.e Checks if the incoming value is null, empty (""), or just spaces (" "). Defaults to "Unknown" if invalid.
    public string Name
    {
        get => name;
        set => name = string.IsNullOrWhiteSpace(value) ? "Unknown" : value.Trim();
    }

    // Telephone property with trimming
    // Defaults to empty string if null
    public string Telephone
    {
        get => telephone;
        
        // Custom setter with explicit null check and trimming
        set
        {
            if (value == null)
            {
                telephone = string.Empty;
            }
            else
            {
                telephone = value.Trim();
            }
        }

        //Alternative concise syntax (I discovered this after further research) - null-conditional operator and null-coalescing operator
        //set => telephone = value?.Trim() ?? string.Empty;
    }

    // Email property with trimming
    // Defaults to empty string if null
    public string Email
    {
        get => email;
        set => email = value?.Trim() ?? string.Empty;
    }

    // Role property (read-only outside, set in constructor)
    // The setter is protected, meaning only this class (Person) and its subclasses (derived classes) can assign a new value to Role.
    // Role is set only once during construction
    public string Role
    {
        get => role;
        protected set => role = value; // controlled encapsulation pattern
    }

    // Constructor (common to all)
    // Initializes Id and Role
    // Other fields can be set via properties
    protected Person(int id, string role) // Protected constructor to prevent direct instantiation
    {
        Id = id; // Set unique identifier
        Role = role; // Set role (Teacher/Admin/Student)
    }

    // Virtual method (to be overridden) for displaying info i.e derived classes can override this method to customize display behavior (Polymorphism)
    // Displays common fields
    // Derived classes will append their specific fields
    public virtual void DisplayInfo()
    {
        // Display the core information in formatted columns
        // This provides a consistent layout for all records
        // The derived classes will add their specific details below this line

        // Console.WriteLine($"ID: {Id} | Role: {Role} | Name: {Name} | Tel: {Telephone} | Email: {Email}");
        Console.WriteLine($"{Id,-5} | {Role,-10} | {Name,-20} | {Telephone,-12} | {Email,-25}"); // Formatted output
    }

    // Editable common fields
    // Can be called by derived classes to edit common fields
    // Derived classes can extend this method to edit their specific fields
    public virtual void EditCommonFields()
    {
        Console.Write($"Enter new name (OR Leave blank and press Enter to keep as '{Name}'): ");
        var input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) Name = input;

        Console.Write($"Enter new telephone (OR Leave blank and press Enter to keep as '{Telephone}'): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) Telephone = input;

        Console.Write($"Enter new email (OR Leave blank and press Enter to keep as '{Email}'): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) Email = input;
    }

    // Virtual method to convert object to CSV format for file storage
    // Derived classes will override this to include their specific fields
    public virtual string ToCsvString()
    {
        return $"{Id},{Role},{Name},{Telephone},{Email}";
    }

    // Virtual method to populate object from CSV data
    // Derived classes will override this to handle their specific fields
    public virtual void FromCsv(string[] fields)
    {
        // Skip Id and Role (fields 0 and 1) as they are set in constructor
        Name = fields[2];
        Telephone = fields[3];
        Email = fields[4];
    }
}

// =============================
//  DERIVED CLASS: Teacher
// =============================

// Inherits from Person and adds specific fields for Teacher
public class Teacher : Person
{
    // Private fields for Teacher-specific data
    private double salary;
    private string subject1;
    private string subject2;

    // Public properties for Teacher-specific fields
    // Salary with basic validation (non-negative)
    public double Salary
    {
        get => salary;
        set => salary = value < 0 ? 0 : value;
    }

    public string Subject1
    {
        get => subject1;
        set => subject1 = value?.Trim() ?? string.Empty;
    }

    public string Subject2
    {
        get => subject2;
        set => subject2 = value?.Trim() ?? string.Empty;
    }

    // Constructor
    // Calls base constructor to set Id and Role
    // Other fields can be set via properties
    public Teacher(int id) : base(id, "Teacher") { }

    // Override method to display Teacher-specific info
    // Calls base method to display common fields
    // Appends Teacher-specific fields (Polymorphic behavior)
    public override void DisplayInfo()
    {
        // Call base method to display common fields
        base.DisplayInfo();
        // Append Teacher-specific fields
        Console.WriteLine($"       \u2514\u2500\u2500> Details -> Salary: {Salary,8:C2} | Subjects: {Subject1}, {Subject2}"); // Formatted output. I used Unicode characters for better visual separation (researched extensively on Unicode box-drawing characters)
        Console.WriteLine(); // Blank line for better readability
    }

    // Override method to edit Teacher-specific fields
    // Calls base method to edit common fields
    // Extends to edit Teacher-specific fields
    public override void EditCommonFields()
    {
        // Calls base method to edit common fields
        base.EditCommonFields();

        // Extends to edit Teacher-specific fields (Here, I'm overriding the base method EditCommonFields() to add functionality specific to Teacher)
        Console.Write($"Enter new salary (OR Leave blank and press Enter to keep as '{Salary}'): ");
        var input = Console.ReadLine();
        if (double.TryParse(input, out double s))
            {
                Salary = s;
            };

        Console.Write($"Enter subject 1 (OR Leave blank and press Enter to keep as '{Subject1}'): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
            {
                Subject1 = input;
            };

        Console.Write($"Enter subject 2 (OR Leave blank and press Enter to keep as '{Subject2}'): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
            {
                Subject2 = input;
            };
    }

    // Override method to convert Teacher object to CSV format with specific field names
    public override string ToCsvString()
    {
        // FORMAT: Id,Role,Name,Telephone,Email,Salary,Subject1,Subject2,Subject3,EmploymentType,WorkingHours
        return $"{base.ToCsvString()},{Salary},{Subject1},{Subject2},,,"; // Empty fields for: Subject3, EmploymentType, WorkingHours (Teacher doesn't use these)
    }

    // Override method to populate Teacher object from CSV data
    public override void FromCsv(string[] fields)
    {
        base.FromCsv(fields); // Read common fields first
        
        // NEW FIELD MAPPING for Teacher:
        // 5: Salary, 6: Subject1, 7: Subject2
        if (fields.Length >= 8) // Ensure I have enough fields
        {
            if (double.TryParse(fields[5], out double salary)) Salary = salary;
            Subject1 = fields[6];
            Subject2 = fields[7];
            // Fields 8, 9, 10 are not used by Teacher (Subject3, EmploymentType, WorkingHours)
        }
    }
}

// =============================
//  DERIVED CLASS: Admin
// =============================

// Inherits from Person and adds specific fields for Admin
public class Admin : Person
{
    // Private Admin fields
    private double salary;
    private string employmentType;
    private int workingHours;

    // Public properties for Admin-specific fields
    public double Salary
    {
        get => salary;
        set => salary = value;
    }

    public string EmploymentType
    {
        get => employmentType;
        set => employmentType = value;
    }

    public int WorkingHours
    {
        get => workingHours;
        set => workingHours = value;
    }

    // Constructor - calls base constructor to set Id and Role
    public Admin(int id) : base(id, "Admin") { }

    // Override method to display Admin-specific info
    public override void DisplayInfo()
    {
        // Call base method to display common fields
        base.DisplayInfo();

        // Append Admin-specific fields (Polymorphic behavior)
        // Console.WriteLine($"   Admin Details -> Salary: {Salary}, Type: {EmploymentType}, Hours: {WorkingHours}");
        Console.WriteLine($"       \u2514\u2500\u2500> Details -> Salary: {Salary,8:C2} | Type: {EmploymentType} | Hours: {WorkingHours}"); // Formatted output
        Console.WriteLine();
    }

    // Override method to edit Admin-specific fields
    public override void EditCommonFields()
    {
        // Calls base method to edit common fields
        base.EditCommonFields();

        // Extends to edit Admin-specific fields. (Here, I'm overriding the base method EditCommonFields() to add functionality specific to Admin)
        Console.Write($"Enter new salary (OR Leave blank and press Enter to keep as '{Salary}'): ");
        var input = Console.ReadLine();
        if (double.TryParse(input, out double s))
            {
                Salary = s;
            };

        Console.Write($"Enter employment type (OR Leave blank and press Enter to keep as '{EmploymentType}'): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
            {
                EmploymentType = input;
            };

        Console.Write($"Enter working hours (OR Leave blank and press Enter to keep as '{WorkingHours}'): ");
        input = Console.ReadLine();
        if (int.TryParse(input, out int h))
            {
                WorkingHours = h;
            };
    }

    // Override method to convert Admin object to CSV format with specific field names
    public override string ToCsvString()
    {
        // FORMAT: Id,Role,Name,Telephone,Email,Salary,Subject1,Subject2,Subject3,EmploymentType,WorkingHours
        return $"{base.ToCsvString()},{Salary},,,,{EmploymentType},{WorkingHours}"; // Empty fields for: Subject1, Subject2, Subject3 (Admin doesn't use these)
        
    }

    // Override method to populate Admin object from CSV data
    public override void FromCsv(string[] fields)
    {
        base.FromCsv(fields); // Read common fields first
        
        // NEW FIELD MAPPING for Admin:
        // 5: Salary, 9: EmploymentType, 10: WorkingHours
        if (fields.Length >= 11) // Ensure I have enough fields
        {
            if (double.TryParse(fields[5], out double salary)) Salary = salary;
            EmploymentType = fields[9];
            if (int.TryParse(fields[10], out int hours)) WorkingHours = hours; // Fields 6, 7, 8 are not used by Admin (Subject1, Subject2, Subject3)
        }
    }
}

// =============================
//  DERIVED CLASS: Student
// =============================

// Inherits from Person and adds specific fields for Student
// Student-specific fields: Subject1, Subject2, Subject3
public class Student : Person
{
    // Private Student fields
    private string subject1;
    private string subject2;
    private string subject3;

    // Public properties for Student-specific fields
    public string Subject1
    {
        get => subject1;
        set => subject1 = value;
    }

    public string Subject2
    {
        get => subject2;
        set => subject2 = value;
    }

    public string Subject3
    {
        get => subject3;
        set => subject3 = value;
    }

    // Constructor - calls base constructor to set Id and Role
    public Student(int id) : base(id, "Student") { }

    // Override method to display Student-specific info
    public override void DisplayInfo()
    {
        // Call base method to display common fields
        base.DisplayInfo();

        // Append Student-specific fields (Polymorphic behavior)
        // Console.WriteLine($"   Student Details -> Subjects: {Subject1}, {Subject2}, {Subject3}");
        Console.WriteLine($"       \u2514\u2500\u2500> Details -> Subjects: {Subject1}, {Subject2}, {Subject3}"); // Formatted output
        Console.WriteLine();
    }

    // Override method to edit Student-specific fields
    public override void EditCommonFields()
    {
        // Calls base method to edit common fields
        base.EditCommonFields();

        // Extends to edit Student-specific fields. (Here, I'm overriding the base method EditCommonFields() to add functionality specific to Student)
        Console.Write($"Enter subject 1 (OR Leave blank and press Enter to keep as '{Subject1}'): ");
        var input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
            {
                Subject1 = input;
            };

        Console.Write($"Enter subject 2 (OR Leave blank and press Enter to keep as '{Subject2}'): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
            {
                Subject2 = input;
            };

        Console.Write($"Enter subject 3 (OR Leave blank and press Enter to keep as '{Subject3}'): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
            {
                Subject3 = input;
            };
        
    }

    // Override method to convert Student object to CSV format with specific field names
    public override string ToCsvString()
    {
        // FORMAT: Id,Role,Name,Telephone,Email,Salary,Subject1,Subject2,Subject3,EmploymentType,WorkingHours
        return $"{base.ToCsvString()},,{Subject1},{Subject2},{Subject3},,"; // Empty fields for: Salary, EmploymentType, WorkingHours (Student doesn't use these)
    }

    // Override method to populate Student object from CSV data
    public override void FromCsv(string[] fields)
    {
        base.FromCsv(fields); // Read common fields first
        
        // NEW FIELD MAPPING for Student:
        // 6: Subject1, 7: Subject2, 8: Subject3
        if (fields.Length >= 9) // Ensure i have enough fields
        {
            Subject1 = fields[6];
            Subject2 = fields[7];
            Subject3 = fields[8];
            // Fields 5, 9, 10 are not used by Student (Salary, EmploymentType, WorkingHours)
        }
    }
}


// =========================================
//  PROGRAM ENTRY POINT (Main program Class)
// =========================================

// Main program class
class Program
{
    // Creating a dynamic shared named "people" to store Person objects (any type: Teacher, Admin, Student). Initialized as an empty list.
    // Using List<Person> to leverage polymorphism
    static List<Person> people = new List<Person>();

    static int nextId = 1; // A counter to assign unique IDs

    // Constant for CSV file name
    private const string DataFileName = "education_centre_data.csv";

    // Main method
    static void Main()
    {
        // Load existing data into 'people' list from CSV file when program starts
        LoadDataFromFile();

        bool exit = false; // controls the main loop. Exits when true.

        // Main loop for menu. Exits when user chooses to.(Option 6)
        while (!exit)
        {
            // Display header
            Console.WriteLine(); // Blank line for better readability
            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine("\n EDUCATION CENTRE MANAGEMENT INFORMATION SYSTEM (Desktop App)");
            Console.WriteLine("\n Developed By: Philip Agano - Banner ID: 001461592 | University of Greenwich");
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine("\n Hello Admin, What would you like to do?");
            Console.WriteLine();

            // Display menu options
            Console.WriteLine("1. Add New record");
            Console.WriteLine("2. View All records");
            Console.WriteLine("3. View records by Role");
            Console.WriteLine("4. Edit existing record");
            Console.WriteLine("5. Delete existing record");
            Console.WriteLine("6. Exit Application");
            Console.WriteLine();
            Console.Write("Select an option (1–6): ");

            // Read user choice
            string choice = Console.ReadLine();
            Console.WriteLine(); // Blank line for better readability

            // Handle user choice. Calls appropriate method based on input.
            // Invalid input is handled in default case.
            switch (choice)
            {
                case "1":
                    AddRecord();
                    break;
                case "2":
                    ViewAllRecords();
                    break;
                case "3":
                    ViewRecordsByRole();
                    break;
                case "4":
                    EditRecord();
                    break;
                case "5":
                    DeleteRecord();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again (Allowed Options: 1, 2, 3, 4, 5, 6).");
                    break;
            }
        }

        // Save data to CSV file when program exits
        SaveDataToFile();

        // Exit message
        Console.WriteLine("Exiting System... Goodbye!");
        Console.Beep(); // Audible feedback on exit - I'm including this jus for fun.
    }

    // ==============================
    //  MENU OPTION METHODS. 
    // ==============================

    // Each method corresponds to a menu option.
    // They handle adding, viewing, editing, and deleting records.
    // The methods leverage polymorphism to manage different Person types.

    // ------------------------------
    // MENU OPTION 1: ADD NEW RECORD
    // ------------------------------

    // Method to add a new record
    static void AddRecord()
    {
        Console.WriteLine("Select User Group:");
        Console.WriteLine("1. Teacher");
        Console.WriteLine("2. Admin");
        Console.WriteLine("3. Student");
        Console.Write("Your Choice: ");
        string roleChoice = Console.ReadLine();

        // Variable to hold the new person object. I'm Initializing it to null at the begining.
        Person person = null;

        // The switch decides which class object to create based on user input
        switch (roleChoice)
        {
            //  Add Teacher (Case "1")
            //  Collects Teacher-specific data
            case "1":
            // Create new Teacher object
                person = new Teacher(nextId++);
            // Set common fields
                Console.Write("Enter Teacher's Name: ");
                person.Name = Console.ReadLine();
                Console.Write("Enter Teacher's Telephone No.: ");
                person.Telephone = Console.ReadLine();
                Console.Write("Enter Teacher's Email: ");
                person.Email = Console.ReadLine();

            // Set Teacher-specific fields
                Console.Write("Enter Salary: ");
                if (double.TryParse(Console.ReadLine(), out double tSalary)) // Basic validation for salary input. If parsing fails, salary remains default (0)
                    ((Teacher)person).Salary = tSalary; // Cast to Teacher to access Teacher-specific properties

                Console.Write("Enter Subject 1: ");
                ((Teacher)person).Subject1 = Console.ReadLine();
                Console.Write("Enter Subject 2: ");
                ((Teacher)person).Subject2 = Console.ReadLine();
                break;

            // Add Admin (Case "2")
            // Collects Admin-specific data
            case "2":
            // Create new Admin object
                person = new Admin(nextId++);
            // Set common fields
                Console.Write("Enter Admin's Name: ");
                person.Name = Console.ReadLine();
                Console.Write("Enter Admin's Telephone: ");
                person.Telephone = Console.ReadLine();
                Console.Write("Enter Admin's Email: ");
                person.Email = Console.ReadLine();

            // Set Admin-specific fields
                Console.Write("Enter Salary: ");
                // Basic validation for salary input
                if (double.TryParse(Console.ReadLine(), out double aSalary)) // If parsing fails, salary remains default (0)
                    ((Admin)person).Salary = aSalary;

                // Admin-specific fields
                Console.Write("Employment type (Full-time/Part-time): ");
                ((Admin)person).EmploymentType = Console.ReadLine();

                Console.Write("Working Hours: ");
                // Basic validation for working hours input
                if (int.TryParse(Console.ReadLine(), out int hours)) // If parsing fails, working hours remains default (0)
                    ((Admin)person).WorkingHours = hours;
                break;

            // Add Student (Case "3")
            // Collects Student-specific data
            case "3":
            // Create new Student object
                person = new Student(nextId++);
            // Set common fields
                Console.Write("Enter Student Name: ");
                person.Name = Console.ReadLine();
                Console.Write("Enter Student Telephone No.: ");
                person.Telephone = Console.ReadLine();
                Console.Write("Enter Student Email: ");
                person.Email = Console.ReadLine();

            // Set Student-specific fields
                Console.Write("Enter Subject 1: ");
                ((Student)person).Subject1 = Console.ReadLine();
                Console.Write("Enter Subject 2: ");
                ((Student)person).Subject2 = Console.ReadLine();
                Console.Write("Enter Subject 3: ");
                ((Student)person).Subject3 = Console.ReadLine();
                break;

            default:
                Console.WriteLine("Invalid role selected. Allowed options are: 1 - Teacher, 2 - Admin, or 3 - Student.");
                return;
        }

        // Finally, store the new person in the list
        people.Add(person);
        
        // Save data to file after adding new record to ensure persistence
        SaveDataToFile();
        
        Console.WriteLine($"\nRecord added successfully! Assigned ID: {person.Id}");
    }

    // --------------------------------
    // MENU OPTION 2: VIEW ALL RECORDS
    // --------------------------------

    // Method to view all records
    static void ViewAllRecords()
    {
        // Check if there are any records
        if (people.Count == 0)
        {
            Console.WriteLine("No records found.");
            return;
        }

        // Display all records
        Console.WriteLine("All Records:");
        // Console.WriteLine("-------------------------------------------------------");
        // Console.WriteLine();

        // Print table header for better readability
        PrintTableHeader();

        // Iterate through the list and display each person's info
        foreach (var p in people)
        {
            p.DisplayInfo(); // Polymorphic call – runs the correct derived method.
        }
    }

    // ---------------------------
    // MENU OPTION 3: VIEW BY ROLE
    // ---------------------------

    // Method to view records filtered by role
static void ViewRecordsByRole()
    {
        Console.Write("Enter role to filter (Teacher/Admin/Student): ");
        string role = Console.ReadLine()?.Trim();

        // Check for invalid input
        if (string.IsNullOrWhiteSpace(role) ||
            !(role.Equals("Teacher", StringComparison.OrdinalIgnoreCase) ||
            role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
            role.Equals("Student", StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Invalid role entered! Please type Teacher, Admin, or Student.");
            Console.WriteLine("Returning to main menu...\n");
            return;
        }

        // Filter records based on valid role (case-insensitive)
        var filtered = people.FindAll(p => p.Role.Equals(role, StringComparison.OrdinalIgnoreCase));

        if (filtered.Count == 0)
        {
            Console.WriteLine($"No records found for role: {role}");
            return;
        }

        Console.WriteLine($"\nDisplaying Records for role \"{role}\":");

        PrintTableHeader(); // Print table header for better readability
        foreach (var p in filtered)
        {
            p.DisplayInfo(); // polymorphic call
        }
    }

    // --------------------------
    // MENU OPTION 4: EDIT RECORD
    // --------------------------

    // Method to edit an existing record
    static void EditRecord()
    {
        Console.Write("Enter record ID to edit: ");

        // Validate ID input
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        // Find the person by ID
        var person = people.Find(p => p.Id == id);

        // If not found, inform user
        if (person == null)
        {
            Console.WriteLine("Record not found.");
            return;
        }

        // If record is found, Call the edit method (polymorphic)
        person.EditCommonFields();
        
        // NEW: Save data to file after editing to ensure persistence
        SaveDataToFile();
        
        Console.WriteLine();
        Console.WriteLine("Record updated successfully.");
    }

    // ----------------------------
    // MENU OPTION 5: DELETE RECORD
    // ----------------------------

    // Method to delete an existing record
    static void DeleteRecord()
    {
        Console.Write("Enter record ID to delete: ");

        // Validate ID input
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID entered. Returning to main menu...\n");
            return;
        }

        // Find the person by ID
        var person = people.Find(p => p.Id == id);

        // If not found, inform user
        if (person == null)
        {
            Console.WriteLine($"Record with ID {id} not found.\n");
            return;
        }

        // Display record info before confirming
        Console.WriteLine("\nRecord found:");
        person.DisplayInfo();

        // Ask for confirmation
        Console.Write("\nAre you sure you want to delete this record? (1. Yes, 2. No): ");
        string choice = Console.ReadLine()?.Trim();

        switch (choice)
        {
            case "1":
                people.Remove(person);
                
                // NEW: Save data to file after deletion to ensure persistence
                SaveDataToFile();
                
                Console.WriteLine("Record deleted successfully.\n");
                break;

            case "2":
                Console.WriteLine("Operation cancelled. Record not deleted.\n");
                break;

            default:
                Console.WriteLine("Invalid choice. Operation aborted.\n");
                break;
        }
    }

    // ==============================================
    // FILE INPUT/OUTPUT METHODS FOR DATA PERSISTENCE
    // ==============================================

    // ---------------------------------------------------------------------------------
    // 1. FILE INPUT METHOD: For saving all records to CSV file using StreamWriter
    // This method writes each person's data to a CSV file in a structured format
    // ---------------------------------------------------------------------------------
    static void SaveDataToFile()
    {
        try
        {
            // Using StreamWriter to write data to file
            // The "using" statement ensures proper disposal of resources
            using (StreamWriter writer = new StreamWriter(DataFileName))
            {
                // Write descriptive header with clear column names
                // FORMAT: Id,Role,Name,Telephone,Email,Salary,Subject1,Subject2,Subject3,EmploymentType,WorkingHours
                writer.WriteLine("Id,Role,Name,Telephone,Email,Salary,Subject1,Subject2,Subject3,EmploymentType,WorkingHours");

                // Write each person's data using their ToCsvString method
                foreach (var person in people)
                {
                    writer.WriteLine(person.ToCsvString());
                }
            }
            Console.WriteLine("Data saved successfully."); // For my debugging
        }
        catch (Exception excep)
        {
            Console.WriteLine($"Error while saving data: {excep.Message}");
        }
    }

    // ---------------------------------------------------------------------------------
    // 2. FILE OUTPUT METHOD: For loading records from CSV file using StreamReader
    // This method reads the CSV file and recreates the Person objects
    // ---------------------------------------------------------------------------------
    static void LoadDataFromFile()
    {
        // Check if file exists before trying to read
        if (!File.Exists(DataFileName))
        {
            Console.WriteLine("No existing data file found. Starting with empty records.");
            return;
        }

        try
        {
            // Using StreamReader to read data from file
            // The 'using' statement ensures proper disposal of resources
            using (StreamReader reader = new StreamReader(DataFileName))
            {
                // Read and skip header line
                string header = reader.ReadLine();
                
                // Clear existing data before loading to avoid duplicates
                people.Clear();
                
                // Track the highest ID to set nextId correctly
                int maxId = 0;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Split CSV line into fields
                    string[] fields = line.Split(',');

                    // Ensuring I have at least the basic fields
                    if (fields.Length < 5)
                        continue;

                    // Parse ID and create appropriate object based on role
                    if (int.TryParse(fields[0], out int id))
                    {
                        Person person = null;
                        string role = fields[1];

                        // Create the appropriate derived class based on role
                        switch (role.ToLower())
                        {
                            case "teacher":
                                person = new Teacher(id);
                                break;
                            case "admin":
                                person = new Admin(id);
                                break;
                            case "student":
                                person = new Student(id);
                                break;
                            default:
                                continue; // Skip unknown roles
                        }

                        // Populate the object from CSV data
                        person.FromCsv(fields);

                        // Add to collection
                        people.Add(person);

                        // Update maxId to ensure nextId is correct
                        if (id > maxId)
                            maxId = id;
                    }
                }

                // Set nextId to one more than the highest ID found
                nextId = maxId + 1;
                
                Console.WriteLine($"Data loaded successfully. {people.Count} records found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            Console.WriteLine("Starting with empty records.");
        }
    }

    // -------------------------------------------------------------------------
    // Helper method to print table header for better readability. 
    // Used in ViewAllRecords and ViewRecordsByRole methods
    // Enhances the display of records in a tabular format
    // I implemented this after further research on C# console output formatting
    // --------------------------------------------------------------------------
    static void PrintTableHeader()
    {
        Console.WriteLine("\n------------------------------------------------------------------------------");
        Console.WriteLine($"{"ID",-5} | {"Role",-10} | {"Name",-20} | {"Telephone",-12} | {"Email",-25}"); // Table Header
        Console.WriteLine("------------------------------------------------------------------------------");
    }

}