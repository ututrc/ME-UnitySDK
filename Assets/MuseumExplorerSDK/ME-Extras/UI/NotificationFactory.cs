using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NotificationFactory : MonoBehaviour
{
    public GameObject notificationPrefab;
    public string notificationparentName = "Notifications";
    public float notificationFullAlphaTime = 5;
    public float notificationFadeInSpeed = 10;
    public float notificationFadeOutSpeed = 5;
    public float notificationsSpacing = 50;
    public int maxNumberOfNotifications = 5;

	private HashSet<Text> notifications = new HashSet<Text>();
	private List<Vector2> allPositions = new List<Vector2>();
	private Dictionary<RectTransform, Vector2> reservedPositions = new Dictionary<RectTransform, Vector2>();
	private bool positionsCalculated;

    public Text CreateNotification(string msg, Color? color = null, bool useOutline = true, Color? outlineColor = null, bool onlyOneInstanceAllowed = false)
    {
		if (onlyOneInstanceAllowed)
		{
			Text existingNote = notifications.Where(n => n.text.Equals(msg)).FirstOrDefault();
			if (existingNote != null)
			{
				Debug.Log("[NotificationFactory] An existing notification with the same string found.");
				return existingNote;
			}
		}
        // Instantiate and setup transforms
        var noteGO = Instantiate(notificationPrefab);
		var rectT = noteGO.transform as RectTransform;
		if (!positionsCalculated) { CalculatePositions(rectT.anchoredPosition); }
        GameObject notificationParent = GameObject.Find(notificationparentName);
        if (notificationParent == null)
        {
			Debug.Log(string.Format("[NotificationFactory] Could not find a notification parent with the name {1}. Creating one...", name, notificationparentName));
            notificationParent = new GameObject(notificationparentName);
            var parentTransform = notificationParent.AddComponent<RectTransform>();
            parentTransform.SetParent(GUIManager.Canvas.transform, worldPositionStays: false);
        }
        rectT.SetParent(notificationParent.transform, worldPositionStays: false);
        if (reservedPositions.Count == allPositions.Count)
        {
			Debug.LogWarning("[NotificationFactory] Too many notifications! Please increase the maxNumberOfNotifications or spawn less notifications at a time.");
        }
        // Get the first free position
        var pos = allPositions.FirstOrDefault(p => !reservedPositions.ContainsValue(p));
        rectT.anchoredPosition = pos;
        reservedPositions.Add(rectT, pos);
        // Set colors and effects
        var note = noteGO.GetComponent<Text>();
        color = color ?? Color.white;
        note.color = color.Value;
        if (useOutline)
        {
            var outline = GetComponent<Outline>();
            if (outline == null) { outline = noteGO.AddComponent<Outline>(); }
            outlineColor = outlineColor ?? Color.black;
            outline.effectColor = outlineColor.Value;
        }
        // Set text
        note.text = msg;
        // Set fading and define the destructor as a callback
        var fader = noteGO.GetComponent<UIFader>() ?? noteGO.AddComponent<UIFader>();
        fader.fullAlphaTime = notificationFullAlphaTime;
        fader.FadeIn(1, notificationFadeInSpeed, () => 
            fader.FadeOut(0, notificationFadeOutSpeed, () => 
            {
                notifications.Remove(note);
				reservedPositions.Remove(rectT);
                Destroy(noteGO);
            }));
		notifications.Add(note);
        return note;
    }

	private void CalculatePositions(Vector2 basePosition)
	{
		for (int i = 1; i <= maxNumberOfNotifications; i++)
		{
			int notificationsOnThisSide = Mathf.FloorToInt(i / 2);
			float spacing = notificationsOnThisSide * notificationsSpacing;
			Vector2 newPos = i % 2 == 0 ? 
				new Vector2(basePosition.x, basePosition.y + spacing) :
				new Vector2(basePosition.x, basePosition.y - spacing);
			allPositions.Add(newPos);
		}
		positionsCalculated = true;
	}
}
