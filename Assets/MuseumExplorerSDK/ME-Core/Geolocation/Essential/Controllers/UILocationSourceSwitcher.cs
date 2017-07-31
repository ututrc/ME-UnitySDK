using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using AR.Geolocation;

[RequireComponent(typeof(Dropdown))]
public class UILocationSourceSwitcher : MonoBehaviour
{
    public bool enableGPS;
    public bool enableNimble;
    public GeoLocation fakeLocation;

    private Dropdown dropDown;

    void Awake()
    {
        dropDown = GetComponent<Dropdown>();
        dropDown.options.Clear();
        dropDown.options.Add(new Dropdown.OptionData("Disabled"));
        dropDown.options.Add(new Dropdown.OptionData("Fake"));
        if (enableGPS) { dropDown.options.Add(new Dropdown.OptionData("GPS")); }
#if UNITY_IOS
        if (enableNimble) { dropDown.options.Add(new Dropdown.OptionData("Nimble")); }
#endif
        dropDown.captionText.text = dropDown.options.First().text;

        dropDown.onValueChanged.AddListener(selection =>
        {
			LocationSourceManager.RemoveAllProvidersAndStop();
            string selectedOption = dropDown.options[selection].text;
            if (selectedOption == "Fake")
            {
				fakeLocation = AR.Geolocation.LocationSourceManager.CurrentGeolocation;
                LocationSourceManager.AddProvider(new FakeLocationSource(fakeLocation));
            }
            else if (selectedOption == "GPS")
            {
                LocationSourceManager.AddProvider(new GpsLocationSource());
            }
			#if INDOORGUIDE
            else if (selectedOption == "Nimble")
            {
                LocationSourceManager.AddProvider(new NimbleLocationSource());
            }
			#endif
            LocationSourceManager.StartLocationUpdates();
        });
    }
}
