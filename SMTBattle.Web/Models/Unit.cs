namespace SMTBattle.Web.Models;

public abstract class Unit
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    
    public int Level { get; set; } = 35;

    public int Strenght { get; set; }
    public int Magic { get; set; }
    public int Vitality { get; set; }
    public int Agility { get; set; }
    public int Luck { get; set; } 


    public int HP { get; set; }
    public int MP { get; set; }

    public bool IsDead => HP <= 0;

    public virtual void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP < 0) HP = 0;
    }
}