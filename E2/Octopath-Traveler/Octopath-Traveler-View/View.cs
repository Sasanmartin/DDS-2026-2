namespace Octopath_Traveler_View;

public class View
{
    // Todos los textos deben coincidir exactamente con los de los test del curso.
    private const string Separator = "----------------------------------------";
    private const string CancelOption = "Cancelar";
    private const string PlayerTeamHeader = "Equipo del jugador";
    private const string EnemyTeamHeader = "Equipo del enemigo";
    private const string CurrentRoundTurnsHeader = "Turnos de la ronda";
    private const string NextRoundTurnsHeader = "Turnos de la siguiente ronda";
    private const string TeamFileSelectionMessage = "Elige un archivo para cargar los equipos";
    private const string InvalidTeamMessage = "Archivo de equipos no válido";
    private const string WeaponSelectionMessage = "Seleccione un arma";
    private const string BoostPointsMessage = "Seleccione cuantos BP utilizar";
    private const string FleeMessage = "El equipo de viajeros ha huido!";
    private const string PlayerWinsMessage = "Gana equipo del jugador";
    private const string EnemyWinsMessage = "Gana equipo del enemigo";
    private const string BasicAttackOption = "1: Ataque básico";
    private const string UseSkillOption = "2: Usar habilidad";
    private const string DefendOption = "3: Defender";
    private const string FleeOption = "4: Huir";
    private const string TurnText = "Turno de ";
    private const string SkillSelectionText = "Seleccione una habilidad para ";
    private const string TargetSelectionText = "Seleccione un objetivo para ";
    private const string DefendText = " se defiende";
    private const string BreakingPointText = " entra en Breaking Point";
    private const string HealMessage = "{0} recupera {1} de vida";
    private const string ReviveText = " revive";
    private const string LowerPriorityMessage = "{0} tendrá menor prioridad de turno durante {1} rondas";
    private const string HpResultMessage = "{0} termina con HP:{1}";
    private const string DamageReceivedText = " recibe ";
    private const string DamageTypedText = " de daño de tipo ";
    private const string DamagePhysicalText = " de daño físico";
    private const string DamageElementalText = " de daño elemental";
    private const string DamageUntypedText = " de daño";
    private const string WeaknessText = " con debilidad";

    private readonly AbstractView _view;

    public static View BuildConsoleView()
        => new View(new ConsoleView());

    public static View BuildTestingView(string pathTestScript)
        => new View(new TestingView(pathTestScript));

    public static View BuildManualTestingView(string pathTestScript)
        => new View(new ManualTestingView(pathTestScript));

    private View(AbstractView newView)
    {
        _view = newView;
    }

    public string[] GetScript()
        => _view.GetScript();

    public int SelectTeamFile(IReadOnlyList<string> fileNames)
    {
        _view.WriteLine(TeamFileSelectionMessage);
        for (int i = 0; i < fileNames.Count; i++)
            _view.WriteLine($"{i}: {fileNames[i]}");
        return ReadNumber();
    }

    public void AnnounceInvalidTeam()
        => _view.WriteLine(InvalidTeamMessage);

    public void AnnounceRoundStart(int round)
    {
        WriteSeparator();
        _view.WriteLine($"INICIA RONDA {round}");
    }

    public void ShowBoard(BoardDisplay board)
    {
        WriteSeparator();
        _view.WriteLine(PlayerTeamHeader);
        foreach (TravelerDisplay traveler in board.Travelers)
            _view.WriteLine(FormatTraveler(traveler));
        _view.WriteLine(EnemyTeamHeader);
        foreach (BeastDisplay beast in board.Beasts)
            _view.WriteLine(FormatBeast(beast));
    }

    public void ShowTurnQueues(TurnQueuesDisplay queues)
    {
        ShowTurnList(CurrentRoundTurnsHeader, queues.CurrentRoundNames);
        ShowTurnList(NextRoundTurnsHeader, queues.NextRoundNames);
    }

    public int SelectAction(string travelerName)
    {
        WriteSeparator();
        _view.WriteLine($"{TurnText}{travelerName}");
        _view.WriteLine(BasicAttackOption);
        _view.WriteLine(UseSkillOption);
        _view.WriteLine(DefendOption);
        _view.WriteLine(FleeOption);
        return ReadNumber();
    }

    public int SelectSkill(string travelerName, IReadOnlyList<string> skillNames)
        => SelectOption($"{SkillSelectionText}{travelerName}", skillNames);

    public int SelectWeapon(IReadOnlyList<string> weapons)
        => SelectOption(WeaponSelectionMessage, weapons);

    private int SelectOption(string title, IReadOnlyList<string> options)
    {
        WriteSeparator();
        _view.WriteLine(title);
        for (int i = 0; i < options.Count; i++)
            _view.WriteLine($"{i + 1}: {options[i]}");
        _view.WriteLine($"{options.Count + 1}: {CancelOption}");
        return ReadNumber();
    }

    public int SelectTarget(string travelerName, IReadOnlyList<TargetDisplay> candidates)
    {
        WriteSeparator();
        _view.WriteLine($"{TargetSelectionText}{travelerName}");
        for (int i = 0; i < candidates.Count; i++)
            _view.WriteLine($"{i + 1}: {FormatTarget(candidates[i])}");
        _view.WriteLine($"{candidates.Count + 1}: {CancelOption}");
        return ReadNumber();
    }

    public int AskBoostPoints()
    {
        WriteSeparator();
        _view.WriteLine(BoostPointsMessage);
        return ReadNumber();
    }

    public void AnnounceBasicAttack(string travelerName)
    {
        WriteSeparator();
        _view.WriteLine($"{travelerName} ataca");
    }

    public void AnnounceSkillUse(string actorName, string skillName)
    {
        WriteSeparator();
        _view.WriteLine($"{actorName} usa {skillName}");
    }

    public void AnnounceDamage(DamageReport report)
    {
        if (report.TargetDefending && report.Kind != DamageKind.Untyped)
            _view.WriteLine($"{report.TargetName}{DefendText}");
        _view.WriteLine($"{report.TargetName}{DamageReceivedText}{report.Damage}{DamageDetailText(report)}");
        if (report.EnteredBreakingPoint)
            _view.WriteLine($"{report.TargetName}{BreakingPointText}");
    }

    public void AnnounceHeal(HealReport report)
        => _view.WriteLine(string.Format(HealMessage, report.TargetName, report.Amount));

    public void AnnounceRevive(ReviveReport report)
        => _view.WriteLine($"{report.TargetName}{ReviveText}");

    public void AnnouncePriority(PriorityReport report)
        =>             _view.WriteLine(string.Format(LowerPriorityMessage, report.TargetName, report.Rounds));

    public void AnnounceHpResults(IReadOnlyList<HpReport> reports)
    {
        foreach (HpReport report in reports)
            _view.WriteLine(string.Format(HpResultMessage, report.Name, report.CurrentHp));
    }

    public void AnnounceFlee()
    {
        WriteSeparator();
        _view.WriteLine(FleeMessage);
    }

    public void AnnouncePlayerWins()
    {
        WriteSeparator();
        _view.WriteLine(PlayerWinsMessage);
    }

    public void AnnounceEnemyWins()
    {
        WriteSeparator();
        _view.WriteLine(EnemyWinsMessage);
    }

    private void ShowTurnList(string header, IReadOnlyList<string> names)
    {
        WriteSeparator();
        _view.WriteLine(header);
        for (int i = 0; i < names.Count; i++)
            _view.WriteLine($"{i + 1}.{names[i]}");
    }

    private static string DamageDetailText(DamageReport report)
        => report.Kind switch
        {
            DamageKind.Typed => $"{DamageTypedText}{report.TypeName}{WeaknessDetailText(report)}",
            DamageKind.Physical => DamagePhysicalText,
            DamageKind.Elemental => DamageElementalText,
            _ => DamageUntypedText
        };

    private static string WeaknessDetailText(DamageReport report)
        => report.Weakness ? WeaknessText : "";

    private static string FormatTraveler(TravelerDisplay traveler)
        => $"{traveler.Letter}-{traveler.Name} - HP:{traveler.CurrentHp}/{traveler.MaxHp} " +
           $"SP:{traveler.CurrentSp}/{traveler.MaxSp} BP:{traveler.BoostPoints}";

    private static string FormatBeast(BeastDisplay beast)
        => $"{beast.Letter}-{beast.Name} - HP:{beast.CurrentHp}/{beast.MaxHp} Shields:{beast.Shields}";

    private static string FormatTarget(TargetDisplay target)
        => target switch
        {
            TravelerTargetDisplay traveler =>
                $"{traveler.Name} - HP:{traveler.CurrentHp}/{traveler.MaxHp} " +
                $"SP:{traveler.CurrentSp}/{traveler.MaxSp} BP:{traveler.BoostPoints}",
            BeastTargetDisplay beast =>
                $"{beast.Name} - HP:{beast.CurrentHp}/{beast.MaxHp} Shields:{beast.Shields}",
            _ => target.Name
        };

    private void WriteSeparator()
        => _view.WriteLine(Separator);

    private int ReadNumber()
        => int.Parse(_view.ReadLine());
}
