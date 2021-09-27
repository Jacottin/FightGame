namespace FightGame.Characters {
    public class Damager : Character {

        public Damager(string playerName) {
            _className = "Damager";
            _userName = playerName;
            _lifePoints = 3;
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