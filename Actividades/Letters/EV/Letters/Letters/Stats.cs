namespace Letters;

public class Stats 
    // Creacion de esta clase para que al dia de manaña si se busca agregar una nueva estadistica
    // Solo se tenga que modificar aqui, de lo contrario se tendria que tener las stats en game y
    // se deberia de crear un efecto como:
    // public override void Apply(ref int atk, ref int spd, ref int res, ref int def)
    // Es decir que estarian todos los efectos como referencia que cuando se agregue una estadistica se modificarian todos.

{
    public int Atk { get; set; }
    public int Spd { get; set; }
    public int Res { get; set; }
    public int Def { get; set; }

    public string GetStats() => $"{Atk}-{Spd}-{Res}-{Def}";
}