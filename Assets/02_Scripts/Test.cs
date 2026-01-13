using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] Button[] wrongButtons;
    [SerializeField] Button[] correctButtons;


    private void Start()
    {
        SetButtons();
        PrintHello();

        Action ramda = () => Debug.Log("Hello");
        Action<int> ramda1 = x => Debug.Log(x);
        Action<int, int> ramda2 = (a, b) => Debug.Log(a + b);

        ramda();
        ramda1(1);
        ramda2(1, 2);
    }

    void PrintHello()
    {
        Debug.Log("Hello");
    }

    public void SetButtons()
    {
        for (int i = 0; i < wrongButtons.Length; i++)
        {
            wrongButtons[i].onClick.AddListener(() => print(i));

            wrongButtons[i].onClick.AddListener(PrintIndex(i));
        }

        for (int i = 0; i < correctButtons.Length; i++)
        {
            int index = i;
            correctButtons[index].onClick.AddListener(() => print(index));
        }
    }

    private UnityAction PrintIndex(int i)
    {
        throw new NotImplementedException();
    }
}
