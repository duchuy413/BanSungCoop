using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants {
    public static int LAYER_GROUND {
        get {
            return LayerMask.NameToLayer("Ground");
        }
    }

    public static int LAYER_JUMPING {
        get {
            return LayerMask.NameToLayer("Jump");
        }
    }
}
