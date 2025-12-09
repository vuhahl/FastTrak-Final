using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Gms.Ads;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;

namespace FastTrak.Controls;

public class AdBannerRenderer : ViewRenderer<AdBanner, FrameLayout>
{
    public AdBannerRenderer(Android.Content.Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<AdBanner> e)
    {
        base.OnElementChanged(e);

        if (Control == null)
        {
            var adView = new Android.Gms.Ads.AdView(Context)
            {
                AdSize = AdSize.Banner,
                AdUnitId = "ca-app-pub-3940256099942544/6300978111" // TEST ID
            };

            var layout = new FrameLayout(Context);
            layout.AddView(adView);

            SetNativeControl(layout);

            var request = new AdRequest.Builder().Build();
            adView.LoadAd(request);
        }
    }
}
