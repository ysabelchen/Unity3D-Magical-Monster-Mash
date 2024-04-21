using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(CharacterHealth))]
public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;

    Transform ui;
    Image healthSlider;
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>()) {
            if (c.renderMode == RenderMode.WorldSpace) {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                break;
            }
        }

        //GetComponent<CharacterHealth>().OnHealthChanged += OnHealthChanged;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ui != null) {
            ui.position = target.position;
            ui.forward = -cam.forward;
        }
    }

    public void UpdateHealth(int maxhealth, int currHealth) {
        if (ui != null) {
            ui.gameObject.SetActive(true);

            float healthPercent = currHealth / (float)maxhealth;
            healthSlider.fillAmount = healthPercent;

            if (currHealth <= 0) {
                Destroy(ui.gameObject);
            }
        }
    }
}
