using System;

namespace FightGame.Characters {
    public class Healer : Character {
        /*
         * The healer has 4 life points, 1 attack point
         * and his special attack returns the same amount
         * of damage to his opponent.
         */
        public Healer(string playerName) {
            _className = "Healer";
            _userName = playerName;
            _lifePoints = 4;
            _totalLifePoints = _lifePoints;
            _defendPoints = 0;
            _attackPoints = 1;
            _defaultAttackPoints = _attackPoints;
            _damages = 0;

        }
        
        
        protected override void _SpecialCapacity(Character other) {
            ++_lifePoints;
            _lifePoints = Math.Min(_totalLifePoints, _lifePoints);
        }
    }
}