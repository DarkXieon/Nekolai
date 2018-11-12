using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerEquipmentManager : EquipmentManager
{
    // Update is called once per frame
    private void Update()
    {
        var userInput = Input.GetAxis("Mouse ScrollWheel");
        var userScrollingUp = userInput > 0;
        var userScrollingDown = userInput < 0;

        if (userScrollingUp || userScrollingDown)
        {
            var nextIndex = userScrollingUp
                ? _ownedFirearms.GetNextPositiontWrap(_indexOfEquiped)
                : _ownedFirearms.GetPreviousPositiontWrap(_indexOfEquiped);

            var nextWeapon = _ownedFirearms.ElementAt(nextIndex);

            this.Equip(nextWeapon);

            _equipedFirearm = nextWeapon;

            _indexOfEquiped = nextIndex;
        }
    }
}
