using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte ReferenceDigit { get; set; } = 0;
    public bool IsActive { get; set; } = false;
    public bool Correct { get; set; } = false;
    public bool WrongTry { get; set; } = false;

    [SerializeField]
    private ParticleSystem _psSuccess;

    [SerializeField]
    private ParticleSystem _psFail;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.NUMBER_BALOON_TAG))
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();

            if (IsActive)
            {
                var color = other.GetComponent<Renderer>().material.color;
                if (ReferenceDigit == balloon.Value)
                {
                    //EmitSuccess(color);
                    //balloon.CorrectNumberEvent();
                    balloon.Brettl = this;
                    //IsActive = false;
                    //Correct = true;
                }
                else
                {
                    EmitFailure(color);
                    balloon.WrongNumberEvent();
                    if (IsActive)
                        WrongTry = true;
                }
            }
        }
    }

    void Emit(ParticleSystem ps, Color color, int amount)
    {
        //ps.startColor = color;
        ps.GetComponent<Renderer>().material.color = color;
        ps.Emit(amount);
    }

    public void EmitSuccess(Color color) => Emit(_psSuccess, color, 100);

    public void EmitFailure(Color color) => Emit(_psFail, color, 50);

}