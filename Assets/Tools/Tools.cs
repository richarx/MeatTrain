using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    public static class Tools
    {
        public static Vector3 ToVector3(this Vector2 vector, float z = 0.0f)
        {
            return new Vector3(vector.x, vector.y, z);
        }
    
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static float NormalizeValue(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
        public static float NormalizeValueInRange(float value, float min, float max, float rangeMin, float rangeMax)
        {
            return ((rangeMax - rangeMin) * ((value - min) / (max - min))) + rangeMin;
        }

        public static bool RandomBool()
        {
            return RandomPositiveOrNegative() > 0;
        }
        public static float RandomPositiveOrNegative(float number = 1.0f)
        {
            int random = (Random.Range(0, 2) * 2) - 1;
            return random * number;
        }

        public static Quaternion ToRotation(this Vector2 direction)
        {
            return Quaternion.AngleAxis(DirectionToDegree(direction), Vector3.forward);
        }

        public static float DirectionToDegree(Vector2 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        public static Vector2 RadianToVector2(float radian, float length = 1.0f)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized * length;
        }

        public static Vector2 DegreeToVector2(float degree, float length = 1.0f)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad).normalized * length;
        }

        public static Vector2 AddAngleToDirection(this Vector2 direction, float angle)
        {
            float directionAngle = DirectionToDegree(direction);
            float newAngle = directionAngle + angle;

            return DegreeToVector2(newAngle).normalized;
        }

        public static Vector2 AddRandomAngleToDirection(this Vector2 direction, float minInclusive, float maxInclusive)
        {
            float directionAngle = DirectionToDegree(direction);
            float newAngle = directionAngle + Random.Range(minInclusive, maxInclusive);

            return DegreeToVector2(newAngle).normalized;
        }

        public static IEnumerator Fade(SpriteRenderer sprite, float duration, bool fadeIn, float maxAlpha = 1.0f, bool scaledTime = true)
        {
            float fade = fadeIn ? 0.0f : maxAlpha;
            float timer = duration;
            float increment = maxAlpha / timer;
            Color color = sprite.color;

            while (timer > 0.0f)
            {
                color.a = fade;
                sprite.color = color;

                float delta = scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;

                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;

                yield return null;
            }

            color.a = fadeIn ? maxAlpha : 0.0f;
            sprite.color = color;
        }

        public static IEnumerator Fade(TextMeshPro text, float duration, bool fadeIn, bool scaledTime = true)
        {
            if (duration == 0.0f)
            {
                Color tmp = text.color;
                tmp.a = fadeIn ? 1.0f : 0.0f;
                text.color = tmp;
                yield break;
            }

            float fade = fadeIn ? 0.0f : 1.0f;
            float timer = duration;
            float increment = 1.0f / timer;
            Color color = text.color;

            while (timer > 0.0f)
            {
                color.a = fade;
                text.color = color;

                float delta = scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;

                yield return null;
            }

            color.a = fadeIn ? 1.0f : 0.0f;
            text.color = color;
        }

        public static IEnumerator Fade(Image sprite, float duration, bool fadeIn, float maxFade = 1.0f, bool scaledTime = true)
        {
            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;
            Color color = sprite.color;

            while (timer > 0.0f)
            {
                color.a = fade;
                sprite.color = color;

                float delta = scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;

                yield return null;
            }

            color.a = fadeIn ? maxFade : 0.0f;
            sprite.color = color;
        }

        public static IEnumerator Fade(TextMeshProUGUI text, float duration, bool fadeIn, float maxFade = 1.0f, bool scaledTime = true)
        {
            if (duration == 0.0f)
            {
                Color tmp = text.color;
                tmp.a = fadeIn ? maxFade : 0.0f;
                text.color = tmp;
                yield break;
            }

            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;
            Color color = text.color;

            while (timer > 0.0f)
            {
                color.a = fade;
                text.color = color;

                float delta = scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;

                yield return null;
            }

            color.a = fadeIn ? maxFade : 0.0f;
            text.color = color;
        }
        
        public static IEnumerator Shake(Transform target, float duration, float intensity, bool horizontal = false, bool vertical = false)
        {
            Vector2 previousShake = Vector2.zero;
        
            float timer = 0.0f;
            while (timer <= duration)
            {
                Vector2 direction = Random.insideUnitCircle;

                if (horizontal)
                    direction.y = 0.0f;

                if (vertical)
                    direction.x = 0.0f;

                target.position += ((direction * intensity) - previousShake).ToVector3();
                previousShake = direction * intensity;
            
                yield return null;
                timer += Time.deltaTime;
            }

            target.position -= previousShake.ToVector3();
        }

        public static IEnumerator TweenPosition(Transform target, float x, float y, float duration, bool deactivateOnEnd = false)
        {
            target.gameObject.SetActive(true);
        
            Vector3 targetPosition = new Vector3(x, y, target.position.z);
            Vector3 velocity = Vector3.zero;
        
            while (Vector3.Distance(target.position, targetPosition) >= 0.15f)
            {
                target.position = Vector3.SmoothDamp(target.position, targetPosition, ref velocity, duration);
                yield return null;
            }

            target.position = targetPosition;
        
            if (deactivateOnEnd)
                target.gameObject.SetActive(false);
        }

        public static string GetLastRecordedDirection(Vector2 lastRecordedMove)
        {
            string[] directions = { "Right", "Up", "Left", "Down" };

            float angle = Mathf.Atan2(lastRecordedMove.y, lastRecordedMove.x);
            int direction = Mathf.RoundToInt(4 * angle / (2 * Mathf.PI) + 4) % 4;

            return directions[direction];
        }
        public static int GetLastRecordedDirectionAsInt(Vector2 lastRecordedMove)
        {
            string[] directions = { "Right", "Up", "Left", "Down" };

            float angle = Mathf.Atan2(lastRecordedMove.y, lastRecordedMove.x);
            return Mathf.RoundToInt(4 * angle / (2 * Mathf.PI) + 4) % 4;
        }

        public static float Distance(this Vector2 position, Vector2 other)
        {
            return (other - position).magnitude;
        }
        
        public static float Distance(this Vector3 position, Vector3 other)
        {
            return (other - position).magnitude;
        }
    }
}
