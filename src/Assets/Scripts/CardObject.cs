using System.Collections;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public Sprite cardImage;
    public int[] cardArrows = new int[4];
    Vector3 deafultPosition;
    public bool isPlaced = false;
    [SerializeField]
    bool canMove;
    bool canAnimate;
    float speed = 5f;
    Vector2 targetPosition;
    public CardObject pushingCard = null;
    Direction animateDirection;
    CardObject animatedCardObject = null;
    public string color;
    void Start()
    {
        deafultPosition = transform.position;
        targetPosition = Vector2.zero;
        canMove = false;
        canAnimate = false;
    }

    


    void Update()
    {
        if (canMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
            if (transform.position == new Vector3(targetPosition.x, targetPosition.y, 0))
            {
                if (canAnimate)
                {
                    canMove = false;
                    StartCoroutine(RestartAnimate());
                    return;
                }
                if (pushingCard != null)
                {
                    Board.instance.setFieldValue(pushingCard, pushingCard.deafultPosition);
                    pushingCard = null;
                }
                Board.instance.setFieldValue(this, transform.position);
                GameManager.instance.IsGameEnd();
                canMove = false;
            }

        }

    }

    public void GoToDeafultPosition()
    {
        transform.position = deafultPosition;
    }

    private void OnMouseDrag()
    {
        if (isPlaced) return;
        if (GameManager.instance.blueTurn && color != "blue") return;
        if (!GameManager.instance.blueTurn && color == "blue") return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction pushDirection = Direction.invalid;
        if (IsAboveBoard(mousePosition))
        {
            Vector3 position = GetCardFieldPosition(mousePosition);
            position = new Vector3(position.x, position.y, -1);
            if (Board.instance.IsFieldEmpty(position.x, position.y))
            {
                if (animatedCardObject != null)
                {
                    animatedCardObject.AnimationRestart();
                    animatedCardObject = null;
                }
                transform.position = position;
            }
            else
            {
                CardObject animateCard = Board.instance.GetFieldValue(position);
                if (animateCard != animatedCardObject && animatedCardObject != null)
                {
                    animatedCardObject.AnimationRestart();
                    animatedCardObject = null;
                }
                float diffrenceX = Mathf.Abs(Mathf.Abs(mousePosition.x) - Mathf.Abs(position.x));
                float diffrenceY = Mathf.Abs(Mathf.Abs(mousePosition.y) - Mathf.Abs(position.y));
                if (mousePosition.x > position.x && diffrenceX > diffrenceY)
                {
                    transform.position = new Vector3(position.x + (Board.MIN_X_TO_MOVE / Board.CARD_OFFSET), position.y, position.z);
                    pushDirection = Direction.left;
                }
                else if (mousePosition.x < position.x && diffrenceX > diffrenceY)
                {
                    transform.position = new Vector3(position.x - (Board.MIN_X_TO_MOVE / Board.CARD_OFFSET), position.y, position.z);
                    pushDirection = Direction.right;
                }
                else if (mousePosition.y > position.y && diffrenceY > diffrenceX)
                {
                    transform.position = new Vector3(position.x, position.y + (Board.MIN_X_TO_MOVE / Board.CARD_OFFSET), position.z);
                    pushDirection = Direction.down;
                }
                else
                {
                    transform.position = new Vector3(position.x, position.y - (Board.MIN_X_TO_MOVE / Board.CARD_OFFSET), position.z);
                    pushDirection = Direction.up;
                }

                if (animateCard.animateDirection != 0 && animateCard.animateDirection != pushDirection)
                {
                    animateCard.AnimationRestart();
                    animatedCardObject = null;
                }

                if (!animateCard.canAnimate)
                {
                    if (CanPush(pushDirection, animateCard, cardArrows[(int)pushDirection - 1]))
                    {
                        animatedCardObject = animateCard;
                        animateCard.canAnimate = true;
                        Vector2 vectorDirection = GetVectorDirection(pushDirection);
                        Vector2 animatePosition = new Vector2(Board.MAIN_X * vectorDirection.x, Board.MAIN_Y * vectorDirection.y);
                        animateCard.StartCoroutine(animateCard.Animate(animatePosition, pushDirection));
                    }
                }
            }
        }
        else transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.blueTurn && color != "blue") return;
        if (!GameManager.instance.blueTurn && color == "blue") return;
        if (isPlaced) return;
        if (!IsAboveBoard(transform.position))
        {
            GoToDeafultPosition();
            return;
        }

        Vector3 position = GetCardFieldPosition(transform.position);
        string positionXY = position.x + " " + position.y;
        Board.instance.SetCardPosition(this, positionXY);

    }

    private IEnumerator Animate(Vector2 move, Direction direction)
    {
        animateDirection = direction;
        yield return new WaitForSeconds(0.5f);
        MoveCard(move, direction);
    }

    private IEnumerator RestartAnimate()
    {
        yield return new WaitForSeconds(0.5f);
        GoToDeafultPosition();
        canAnimate = false;
        animateDirection = 0;
    }

    private IEnumerator BadPushAnimation(Vector2 direction)
    {
        float shift = 0.05f;
        for (int i = 0; i < 5; i++)
        {
            transform.position = new Vector3(transform.position.x + (shift* Mathf.Abs(direction.x)), transform.position.y + (shift* Mathf.Abs(direction.y)), transform.position.z);
            shift = -shift;
            yield return new WaitForSeconds(0.08f);
            transform.position = new Vector3(transform.position.x + (shift * Mathf.Abs(direction.x)), transform.position.y + (shift * Mathf.Abs(direction.y)), transform.position.z);
        }
        GoToDeafultPosition();
    }

    public void AnimationRestart()
    {
        if (canAnimate)
        {
            canMove = false;
            canAnimate = false;
            StopAllCoroutines();
            GoToDeafultPosition();
            Vector2 vectorDirection = GetVectorDirection(animateDirection);
            Vector3 nextCardPosition = new Vector2(transform.position.x + (vectorDirection.x * Board.MAIN_X), transform.position.y + (vectorDirection.y * Board.MAIN_Y));
            CardObject nextCard = Board.instance.GetFieldValue(nextCardPosition);
            if (nextCard != null)
            {
                nextCard.AnimationRestart();
            }
            animateDirection = 0;
        }
    }

    public void SetDeafultPosition(float x, float y)
    {
        deafultPosition = new Vector3(x, y, 0);
    }

    public Vector2 GetDeafultPosition() 
    {
        return deafultPosition;
    }

    bool IsAboveBoard(Vector3 position)
    {
        if (position.x > Board.BOARD_MAX_X || position.x < -Board.BOARD_MAX_X || position.y > Board.BOARD_MAX_Y || position.y < -Board.BOARD_MAX_Y)
        {
            return false;
        }
        else return true;
    }

    Vector3 GetCardFieldPosition(Vector3 cardPosition)
    {
        Vector3 position = Vector3.zero;

        if (cardPosition.x > Board.MIN_X_TO_MOVE)
        {
            position = new Vector3(Board.MAIN_X, position.y, position.z);
        }
        else if (cardPosition.x < -Board.MIN_X_TO_MOVE)
        {
            position = new Vector3(-Board.MAIN_X, position.y, position.z);
        }

        if (cardPosition.y > Board.MIN_Y_TO_MOVE)
        {
            position = new Vector3(position.x, Board.MAIN_Y, position.z);
        }
        else if (cardPosition.y < -Board.MIN_Y_TO_MOVE)
        {
            position = new Vector3(position.x, -Board.MAIN_Y, position.z);
        }
        return position;
    }

    public void SetCardPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void MoveCard(Vector2 move, Direction direction)
    {
        Vector2 targetPosition = new Vector2(transform.position.x + move.x, transform.position.y + move.y);
        this.targetPosition = targetPosition;
        Vector2 vectorDirection = GetVectorDirection(direction);
        Vector3 nextCardPosition = new Vector2(transform.position.x + (vectorDirection.x * Board.MAIN_X), transform.position.y + (vectorDirection.y * Board.MAIN_Y));
        CardObject nextCard = Board.instance.GetFieldValue(nextCardPosition);
        if (nextCard != null)
        {
            if (canAnimate) nextCard.canAnimate = true;
            if (canAnimate) nextCard.animateDirection = direction;
            nextCard.MoveCard(move, direction);
        }
        canMove = true;

    }
    public void CheckPushCard(CardObject pushedCardObject)
    {
        pushedCardObject.AnimationRestart();
        if (transform.position.y < pushedCardObject.transform.position.y)
        {
            pushCard(Direction.up, pushedCardObject);
        }
        else if (transform.position.y > pushedCardObject.transform.position.y)
        {
            pushCard(Direction.down, pushedCardObject);
        }
        else if (transform.position.x > pushedCardObject.transform.position.x)
        {
            pushCard(Direction.left, pushedCardObject);
        }
        else if (transform.position.x < pushedCardObject.transform.position.x)
        {
            pushCard(Direction.right, pushedCardObject);
        }
    }

    void pushCard(Direction direction, CardObject pushedCardObject)
    {
        Vector2 vectorDirection = GetVectorDirection(direction);
        if (CanPush(direction, pushedCardObject, cardArrows[(int)direction - 1]))
        {
            FindObjectOfType<Deck>().RemoveFromHand(this, color);
            FindObjectOfType<Deck>().DrawCard(deafultPosition, color);
            Vector2 pushedCardObjectNewPosition = new Vector2(Board.MAIN_X * vectorDirection.x, Board.MAIN_Y * vectorDirection.y);
            Vector3 pushedCardObjectOldPosition = pushedCardObject.transform.position;
            pushedCardObject.pushingCard = this;
            pushedCardObject.MoveCard(pushedCardObjectNewPosition, direction);
            SetDeafultPosition(pushedCardObjectOldPosition.x, pushedCardObjectOldPosition.y);
            GameManager.instance.ChangeTurn();
        }
        else
        {
            if (direction == Direction.left) direction--;
            else direction++;
            StartCoroutine(BadPushAnimation(GetVectorDirection(direction)));
        }
    }

    public bool CanPush(Direction direction, CardObject card, int arrow)
    {
        if (arrow == 0) return false;
        Direction oppositeDirection = GetOppositeDirection(direction);
        Vector3 cardPosition = GetCardFieldPosition(transform.position);
        Vector2 vectorDirection = GetVectorDirection(direction);
        Vector3 nextCardPosition = new Vector3(cardPosition.x + (Board.MAIN_X * vectorDirection.x), cardPosition.y + (Board.MAIN_Y * vectorDirection.y), 0);
        if (Mathf.Abs(transform.position.x) % Board.MAIN_X != 0 || Mathf.Abs(transform.position.y) % Board.MAIN_Y != 0)
        {
            nextCardPosition = new Vector3(cardPosition.x, cardPosition.y);
        }
        if(Mathf.Abs(transform.position.x) > Board.MAIN_X*2) 
        {
            nextCardPosition = card.transform.position;
        }
        if (Board.instance.IsFieldEmpty(nextCardPosition.x, nextCardPosition.y)) return true;
        if (arrow <= card.cardArrows[(int)oppositeDirection - 1])
        {
            return false;
        }
        else
        {
            CardObject nextCard = Board.instance.GetFieldValue(new Vector2(nextCardPosition.x, nextCardPosition.y));
            if (Mathf.Abs(transform.position.x) % Board.MAIN_X != 0 || Mathf.Abs(transform.position.y) % Board.MAIN_Y != 0)
            {
                nextCard = Board.instance.GetFieldValue(nextCardPosition);
            }
            if (Mathf.Abs(nextCard.transform.position.x) == Board.MAIN_X * 2 || Mathf.Abs(nextCard.transform.position.y) == Board.MAIN_Y * 2) return false;
            return card.CanPush(direction, nextCard, arrow);

        }
    }

    Direction GetOppositeDirection(Direction direction)
    {
        switch (direction) 
        {
            case Direction.up: return Direction.down;
            case Direction.down: return Direction.up;
            case Direction.right: return Direction.left;
            case Direction.left: return Direction.right;
            default: return Direction.invalid;
        }
    }

    Vector2 GetVectorDirection(Direction direction)
    {
        Vector2[] vectorArray = new Vector2[4] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
        return vectorArray[(int)direction - 1];
    }


}

public enum Direction 
{
    invalid,
    up,
    right,
    down,
    left
}
