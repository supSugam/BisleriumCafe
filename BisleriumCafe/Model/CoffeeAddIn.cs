

namespace BisleriumCafe.Model;
public class CoffeeAddIn : IModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AddInName { get; set; }
    public string AddInDescription { get; set; }
    public int AddInPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; }

    public CoffeeAddIn(string addInName, string addInDescription, int addInPrice)
    {
        AddInName = addInName;
        AddInDescription = addInDescription;
        AddInPrice = addInPrice;
    }

}
