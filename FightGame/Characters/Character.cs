using System;

namespace FightGame.Characters {
    public abstract class Character {
        /*
         * Character is an abstract class describing the general behavior
         * of a playable character, providing a set of methods necessary
         * for the proper conduct of a game.
         */
        protected string _className;
        protected string _userName;
        protected int _lifePoints;
        protected int _totalLifePoints;
        protected int _defendPoints;
        protected int _defaultAttackPoints;
        protected int _attackPoints;
        protected int _damages;
        private bool _specialCapacityUsed;
        public AttackType LastAction;
        
        
        public void Attack(Character other) {
            other._damages = _attackPoints;
        }

        public void Defend(Character other) {
            _defendPoints = other._defaultAttackPoints;
        }

        public void SetActualDamage(int otherDamages)
        {
            _damages = otherDamages;
        }
        
        public void Update(Character other) {
            if (_specialCapacityUsed) {
                _SpecialCapacity(other);
                _specialCapacityUsed = false;
            }
        }

        public void ComputeDamages() {
            if (_damages > 0) {
                _lifePoints = _lifePoints + (_defendPoints - _damages);
            }
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

        public int getLife() {
            return _lifePoints;
        }

        public int getTotalLife() {
            return _totalLifePoints;
        }

        protected abstract void _SpecialCapacity(Character other);
    }
}