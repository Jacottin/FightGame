namespace FightGame.Characters {
    public class Damager : Character {
        /*
         * The damager has 3 life points, 2 attack points
         * and his special attack returns the same amount
         * of damage to his opponent.
         */
        public Damager(string playerName) {
            _className = "Damager";
            _userName = playerName;
            _lifePoints = 3;
            _totalLifePoints = _lifePoints;
            _defendPoints = 0;
            _attackPoints = 2;
            _defaultAttackPoints = _attackPoints;
            _damages = 0;

        }
        
        
        protected override void _SpecialCapacity(Character other) {
            other.SetActualDamage(_damages);
        } 
    }
}