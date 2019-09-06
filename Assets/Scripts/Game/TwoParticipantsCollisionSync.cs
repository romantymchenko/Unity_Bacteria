using System;
using System.Collections.Generic;

public static class TwoParticipantsCollisionSync
{
    private static readonly List<Tuple<BactaController, BactaController>> collisions = new List<Tuple<BactaController, BactaController>>();

    public static void OnCollisionHappen(BactaController controllerSelf, BactaController otherController)
    {
        //find existing
        var col = collisions.Find(item => item.Item2 == controllerSelf && item.Item1 == otherController);
        if (col == null)
        {
            collisions.Add(new Tuple<BactaController, BactaController>(controllerSelf, otherController));
        }
        else
        {
            collisions.Remove(col);
            //do cross-call
            if (controllerSelf.PlayerType != ObjectType.FRIEND)
            {
                var otherHP = otherController.Health;
                otherController.DoImpact(controllerSelf.PlayerType, controllerSelf.Health);
                controllerSelf.DoImpact(otherController.PlayerType, otherHP);
            }
            else
            {
                var selfHP = controllerSelf.Health;
                controllerSelf.DoImpact(otherController.PlayerType, otherController.Health);
                otherController.DoImpact(controllerSelf.PlayerType, selfHP);
            }
        }
    }
}
