using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidGainImage : MonoBehaviour
{
    public Text text;
    public void getPic()
    {
        StartCoroutine(GetPictureEn());
    }

    private IEnumerator GetPictureEn()
    {
        // end frame 
        yield return new WaitForEndOfFrame();

        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image
                var pathArray = path.Split('/');
                string output = "";

                foreach(string seg in  pathArray)
                {
                    output = seg;
                }

                output = output.Substring(16, output.Length - 47);
                text.text = output;
            }
        });

    }
}
