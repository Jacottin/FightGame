using System;

namespace FightGame.Characters {
    public abstract class Character {
        protected string _className;
        protected string _userName;
        protected int _lifePoints;
        protected int _defendPoints;
        protected int _defaultAttackPoints;
        protected int _attackPoints;
        protected int _damages;
        private bool _specialCapacityUsed;
        
        
        public void Attack(Character other) {
            other._damages = _attackPoints;
        }

        public void Defend(Character other) {
            _defendPoints = other._defaultAttackPoints;
        }

        public void Update(Character other) {
            if (_specialCapacityUsed) {
                _SpecialCapacity(other);
                _specialCapacityUsed = false;
            }
            _lifePoints = _lifePoints + (_defendPoints - _damages);
            _damages = 0;
            _defendPoints = 0;

        }

        public void SpecialCapacity() {
            _specialCapacityUsed = true;
        }

        public string getClassName() {
            return _className;
        }

        public string getUserName() {
            return _userName;
        }

        protected abstract void _SpecialCapacity(Character other);
    }
}