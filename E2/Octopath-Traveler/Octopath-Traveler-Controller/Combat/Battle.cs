using Octopath_Traveler_View;

namespace Octopath_Traveler;

public class Battle
{
    private const string Separator = "----------------------------------------";
    private const double BasicAttackModifier = 1.3;
    private const string CancelOption = "Cancelar";
    private const string PlayerTeamHeader = "Equipo del jugador";
    private const string EnemyTeamHeader = "Equipo del enemigo";
    private const string CurrentRoundTurnsHeader = "Turnos de la ronda";
    private const string NextRoundTurnsHeader = "Turnos de la siguiente ronda";
    private const string ChooseWeaponMessage = "Seleccione un arma";
    private const string ChooseBoostMessage = "Seleccione cuantos BP utilizar";
    private const string FleeMessage = "El equipo de viajeros ha huido!";
    private const string PlayerWinsMessage = "Gana equipo del jugador";
    private const string EnemyWinsMessage = "Gana equipo del enemigo";

    private const int BasicAttackChoice = 1;
    private const int UseSkillChoice = 2;
    private const int DefendChoice = 3;
    private const int FleeChoice = 4;

    private readonly View _view;
    private readonly List<Traveler> _travelers;
    private readonly List<Beast> _beasts;
    private readonly TurnQueue _turnQueue = new();
    private readonly DamageCalculator _damageCalculator = new();

    private List<Unit> _currentRoundOrder = new();
    private int _currentRoundIndex;

    public Battle(View view, List<Traveler> travelers, List<Beast> beasts)
    {
        _view = view;
        _travelers = travelers;
        _beasts = beasts;
    }

    public void Run()
    {
        int round = 0;
        while (true)
        {
            round++;
            StartNewRound(round);

            while (true)
            {
                Unit? unit = GetNextAliveUnit();
                if (unit is null)
                    break;

                PrintBoard();
                PrintTurnQueues();

                if (PlayTurn(unit))
                    return;
                _currentRoundIndex++;
            }

            GrantBoostPoints();
        }
    }

    private void StartNewRound(int round)
    {
        PrintSeparator();
        _view.WriteLine($"INICIA RONDA {round}");
        _currentRoundOrder = BuildTurnOrder();
        _currentRoundIndex = 0;
    }

    private Unit? GetNextAliveUnit()
    {
        while (_currentRoundIndex < _currentRoundOrder.Count &&
               !_currentRoundOrder[_currentRoundIndex].IsAlive)
            _currentRoundIndex++;
        return _currentRoundIndex < _currentRoundOrder.Count
            ? _currentRoundOrder[_currentRoundIndex]
            : null;
    }

    private bool PlayTurn(Unit unit)
        => unit switch
        {
            Traveler traveler => PlayTravelerTurn(traveler),
            Beast beast => PlayBeastTurn(beast),
            _ => false
        };

    private bool PlayTravelerTurn(Traveler traveler)
    {
        while (true)
        {
            ShowActionMenu(traveler);
            int action = ReadNumber();

            if (action == BasicAttackChoice)
            {
                TurnOutcome outcome = PerformBasicAttack(traveler);
                if (outcome == TurnOutcome.Cancelled)
                    continue;
                return outcome == TurnOutcome.CombatFinished;
            }

            if (action == UseSkillChoice)
                ShowSkillMenu(traveler);

            if (action == FleeChoice)
            {
                PrintSeparator();
                _view.WriteLine(FleeMessage);
                AnnounceWinner(Winner.Enemy);
                return true;
            }
        }
    }

    private bool PlayBeastTurn(Beast beast)
    {
        Traveler target = GetHighestHpTraveler();
        int damage = _damageCalculator.CalculateDamage(beast, target, beast.Skill.Modifier);
        target.TakeDamage(damage);

        PrintSeparator();
        _view.WriteLine($"{beast.Name} usa {beast.Skill.Name}");
        _view.WriteLine($"{target.Name} recibe {damage} de daño físico");
        _view.WriteLine($"{target.Name} termina con HP:{target.CurrentHp}");

        if (AreAllTravelersDead())
        {
            AnnounceWinner(Winner.Enemy);
            return true;
        }
        return false;
    }

    private TurnOutcome PerformBasicAttack(Traveler traveler)
    {
        string? weapon = ChooseWeapon(traveler);
        if (weapon is null)
            return TurnOutcome.Cancelled;

        Beast? target = ChooseTarget(traveler);
        if (target is null)
            return TurnOutcome.Cancelled;

        ChooseBoostPoints(traveler);

        int damage = _damageCalculator.CalculateDamage(traveler, target, BasicAttackModifier);
        target.TakeDamage(damage);

        PrintSeparator();
        _view.WriteLine($"{traveler.Name} ataca");
        _view.WriteLine($"{target.Name} recibe {damage} de daño de tipo {weapon}");
        _view.WriteLine($"{target.Name} termina con HP:{target.CurrentHp}");

        if (AreAllBeastsDead())
        {
            AnnounceWinner(Winner.Player);
            return TurnOutcome.CombatFinished;
        }
        return TurnOutcome.TurnFinished;
    }

    private void ShowActionMenu(Traveler traveler)
    {
        PrintSeparator();
        _view.WriteLine($"Turno de {traveler.Name}");
        _view.WriteLine($"{BasicAttackChoice}: Ataque básico");
        _view.WriteLine($"{UseSkillChoice}: Usar habilidad");
        _view.WriteLine($"{DefendChoice}: Defender");
        _view.WriteLine($"{FleeChoice}: Huir");
    }

    private void ShowSkillMenu(Traveler traveler)
        => ChooseOption($"Seleccione una habilidad para {traveler.Name}", traveler.ActiveSkills);

    private string? ChooseWeapon(Traveler traveler)
    {
        int choice = ChooseOption(ChooseWeaponMessage, traveler.Weapons);
        if (choice == traveler.Weapons.Count + 1)
            return null;
        return traveler.Weapons[choice - 1];
    }

    private Beast? ChooseTarget(Traveler attacker)
    {
        List<Beast> livingBeasts = _beasts.Where(beast => beast.IsAlive).ToList();
        List<string> options = livingBeasts.Select(GetBeastTargetDescription).ToList();

        int choice = ChooseOption($"Seleccione un objetivo para {attacker.Name}", options);
        if (choice == options.Count + 1)
            return null;
        return livingBeasts[choice - 1];
    }

    private int ChooseOption(string title, IReadOnlyList<string> options)
    {
        PrintSeparator();
        _view.WriteLine(title);
        for (int i = 0; i < options.Count; i++)
            _view.WriteLine($"{i + 1}: {options[i]}");
        _view.WriteLine($"{options.Count + 1}: {CancelOption}");
        return ReadNumber();
    }

    private void ChooseBoostPoints(Traveler traveler)
    {
        if (traveler.BoostPoints < 1)
            return;
        PrintSeparator();
        _view.WriteLine(ChooseBoostMessage);
        ReadNumber();
    }

    private Traveler GetHighestHpTraveler()
        => _travelers.Where(traveler => traveler.IsAlive)
            .OrderByDescending(traveler => traveler.CurrentHp)
            .First();

    private void GrantBoostPoints()
    {
        foreach (Traveler traveler in _travelers)
            if (traveler.IsAlive)
                traveler.GrantBoostPoint();
    }

    private bool AreAllBeastsDead()
        => _beasts.All(beast => !beast.IsAlive);

    private bool AreAllTravelersDead()
        => _travelers.All(traveler => !traveler.IsAlive);

    private void PrintBoard()
    {
        PrintSeparator();
        _view.WriteLine(PlayerTeamHeader);
        for (int i = 0; i < _travelers.Count; i++)
            _view.WriteLine(GetTravelerDescription(_travelers[i], (char)('A' + i)));
        _view.WriteLine(EnemyTeamHeader);
        for (int i = 0; i < _beasts.Count; i++)
            _view.WriteLine(GetBeastDescription(_beasts[i], (char)('A' + i)));
    }

    private void PrintTurnQueues()
    {
        PrintTurnList(GetRemainingUnitsInCurrentRound(), CurrentRoundTurnsHeader);
        PrintTurnList(BuildTurnOrder(), NextRoundTurnsHeader);
    }

    private void PrintTurnList(List<Unit> units, string header)
    {
        PrintSeparator();
        _view.WriteLine(header);
        for (int i = 0; i < units.Count; i++)
            _view.WriteLine($"{i + 1}.{units[i].Name}");
    }

    private List<Unit> GetRemainingUnitsInCurrentRound()
        => _currentRoundOrder.Skip(_currentRoundIndex).Where(unit => unit.IsAlive).ToList();

    private List<Unit> BuildTurnOrder()
        => _turnQueue.BuildOrder(
            _travelers.Where(traveler => traveler.IsAlive).ToList(),
            _beasts.Where(beast => beast.IsAlive).ToList());

    private string GetTravelerDescription(Traveler traveler, char letter)
        => $"{letter}-{traveler.Name} - HP:{traveler.CurrentHp}/{traveler.MaxHp} " +
           $"SP:{traveler.CurrentSp}/{traveler.MaxSp} BP:{traveler.BoostPoints}";

    private string GetBeastDescription(Beast beast, char letter)
        => $"{letter}-{beast.Name} - HP:{beast.CurrentHp}/{beast.MaxHp} Shields:{beast.Shields}";

    private string GetBeastTargetDescription(Beast beast)
        => $"{beast.Name} - HP:{beast.CurrentHp}/{beast.MaxHp} Shields:{beast.Shields}";

    private void AnnounceWinner(Winner winner)
    {
        PrintSeparator();
        _view.WriteLine(winner == Winner.Player ? PlayerWinsMessage : EnemyWinsMessage);
    }

    private void PrintSeparator()
        => _view.WriteLine(Separator);

    private int ReadNumber()
        => int.Parse(_view.ReadLine());

    private enum Winner
    {
        Player,
        Enemy
    }

    private enum TurnOutcome
    {
        Cancelled,
        TurnFinished,
        CombatFinished
    }
}
