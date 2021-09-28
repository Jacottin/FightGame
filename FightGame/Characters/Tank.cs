namespace FightGame.Characters {
    public class Tank : Character {

        public Tank(string playerName) {
            _className = "Tank";
            _userName = playerName;
            _lifePoints = 5;
            _totalLifePoints = _lifePoints;
            _defendPoints = 0;
            _attackPoints = 1;
            _defaultAttackPoints = _attackPoints;
            _damages = 0;

        }
        
        
        protected override void _SpecialCapacity(Character other)
        {
            _lifePoints -= 1;
            _attackPoints++;
            Attack(other);
            _attackPoints--;
        }
    }
}