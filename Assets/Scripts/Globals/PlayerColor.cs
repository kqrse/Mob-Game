using UnityEngine;

namespace Globals {
    public static class PlayerColor {
        public static Color PlayerOneActive = new(1, 1, 1, 1);
        public static Color PlayerOneAsleep = new(0.7255f, 0.7255f, 0.7255f, 1);

        public static Color PlayerTwoActive = new(0.3922f, 0.7255f, 0.3922f, 1);
        public static Color PlayerTwoAsleep = new(0.3922f, 0.7255f, 0.3922f, 1);

        public static Color PlayerThreeActive = new(0.3922f, 0.3922f, 0.7255f, 1);
        public static Color PlayerThreeAsleep = new(0.3922f, 0.3922f, 0.7255f, 1);

        public static Color PlayerFourActive = new(0.7255f, 0.3922f, 0.3922f, 1);
        public static Color PlayerFourAsleep = new(0.7255f, 0.3922f, 0.3922f, 1);
    }
}