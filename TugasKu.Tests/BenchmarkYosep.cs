using System.Diagnostics;
// ...
var sw = Stopwatch.StartNew();
var task = new TaskItem { Name = "Valid", Deadline = DateTime.Today.AddDays(1) };
for (int i = 0; i < 50000; i++)
{
    GenericValidator<TaskItem>.IsValid(task, t => !string.IsNullOrEmpty(t.Name));
    var scaled = UIScaler.ScaleFont(new Font("Segoe UI", 10));
}
sw.Stop();
Console.WriteLine($"Yosep (Generics Validator + UI Scaling): {sw.ElapsedMilliseconds} ms");