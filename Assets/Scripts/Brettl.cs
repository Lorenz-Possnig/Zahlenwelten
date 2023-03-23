using UnityEngine;

public class Brettl : MonoBehaviour
{
    public byte ReferenceDigit { get => referenceDigit; set => referenceDigit = value; }
    public byte referenceDigit = 0;
    public bool active = false;
    public bool IsActive { get => active; set => active = value; }
    public bool correct = false;
    public bool Correct { get => correct; set => correct = value; }
    public bool wrongTry = false;
    public bool WrongTry { get => wrongTry; set => wrongTry = value; }

    public bool IsEmpty() => !correct && !wrongTry;

    public Brettl Predecessor;

    [SerializeField]
    private ParticleSystem _psSuccess;

    [SerializeField]
    private ParticleSystem _psFail;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.NUMBER_BALOON_TAG))
        {
            NumberBalloon balloon = other.gameObject.GetComponent<NumberBalloon>();

            /*if (IsActive)
            {
                var color = other.GetComponent<Renderer>().material.color;
                var wrong = Predecessor != null && Predecessor.IsEmpty();
                wrong = wrong && ReferenceDigit == balloon.Value;
                if (!wrong)
                {
                    balloon.Brettl = this;
                }
                else
                {
                    EmitFailure(color);
                    balloon.WrongNumberEvent();
                    if (IsActive)
                        WrongTry = true;
                }
            }*/
            if (IsActive)
            {
                var color = other.GetComponent<Renderer>().material.color;
                var errors = false;
                if (Predecessor != null && Predecessor.IsEmpty())
                {
                    errors = true;
                }
                if (ReferenceDigit != balloon.Value)
                {
                    errors = true;
                }
                if (errors)
                {
                    EmitFailure(color);
                    balloon.WrongNumberEvent();
                    if (IsActive)
                        WrongTry = true;
                } else
                {
                    balloon.Brettl = this;
                }
            }
        }
    }

    void Emit(ParticleSystem ps, Color color, int amount)
    {
        ps.startColor = color;
        ps.GetComponent<Renderer>().material.color = color;
        ps.Emit(amount);
    }

    public void EmitSuccess(Color color) => Emit(_psSuccess, color, 100);

    public void EmitFailure(Color color) => Emit(_psFail, color, 50);

}