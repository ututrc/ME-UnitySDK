using UnityEngine;
using UnityEngine.UI;
using AR.Geolocation;

[RequireComponent(typeof(Toggle))]
public class UILocationSourceAdder : MonoBehaviour
{
    public SourceType sourceType = SourceType.fake;

	private ILocationProvider locationProvider;
	private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
		toggle.isOn = false;
        switch (sourceType)
        {
            case SourceType.fake:
                //locationProvider = new FakeLocationSource();
                break;
            case SourceType.gps:
                locationProvider = new GpsLocationSource();
                break;
			#if INDOORGUIDE
            case SourceType.nimble:
                locationProvider = new NimbleLocationSource();
                break;
			#endif
            default:
                //locationProvider = new FakeLocationSource();
                break;
        }
		toggle.onValueChanged.AddListener(enabled =>
		{
			if (enabled)
			{
				LocationSourceManager.AddProvider(locationProvider);
			}
			else
			{
				LocationSourceManager.RemoveProvider(locationProvider);
			}
		});
    }
}
