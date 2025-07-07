using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float movementSpeed;
    [SerializeField] Animator animator;
    [SerializeField] UIController uiController;
    Dictionary<string, Vector3> letterPositions = new Dictionary<string, Vector3>();
    List<string> letterSequence = new List<string>();
    private Vector3 targetPos;
    private bool needToMove = false;
    private bool isWaiting = false;
    private bool isMovingToYesOrNo = false;

    private void InitializeLetterPositions()
    {
        letterPositions.Add("a", new Vector3(-5.8f, -0.84f, 0f));
        letterPositions.Add("b", new Vector3(-4.83f, -0.62f, 0f));
        letterPositions.Add("c", new Vector3(-3.84f, -0.46f, 0f));
        letterPositions.Add("d", new Vector3(-2.87f, -0.32f, 0f));
        letterPositions.Add("e", new Vector3(-1.9f, -0.16f, 0f));
        letterPositions.Add("f", new Vector3(-0.89f, 0.04f, 0f));
        letterPositions.Add("g", new Vector3(0.05f, 0.06f, 0f));
        letterPositions.Add("h", new Vector3(1.06f, -0.01f, 0f));
        letterPositions.Add("i", new Vector3(2.05f, -0.14f, 0f));
        letterPositions.Add("j", new Vector3(3.08f, -0.27f, 0f));
        letterPositions.Add("k", new Vector3(4.01f, -0.45f, 0f));
        letterPositions.Add("l", new Vector3(4.93f, -0.61f, 0f));
        letterPositions.Add("m", new Vector3(6.03f, -0.81f, 0f));
        letterPositions.Add("n", new Vector3(-6.3f, -2.98f, 0f));
        letterPositions.Add("o", new Vector3(-5.21f, -2.79f, 0f));
        letterPositions.Add("p", new Vector3(-4.18f, -2.61f, 0f));
        letterPositions.Add("q", new Vector3(-3.17f, -2.48f, 0f));
        letterPositions.Add("r", new Vector3(-2.25f, -2.35f, 0f));
        letterPositions.Add("s", new Vector3(-1.37f, -2.15f, 0f));
        letterPositions.Add("t", new Vector3(-0.45f, -1.95f, 0f));
        letterPositions.Add("u", new Vector3(0.43f, -2.15f, 0f));
        letterPositions.Add("v", new Vector3(1.58f, -2.29f, 0f));
        letterPositions.Add("w", new Vector3(2.82f, -2.47f, 0f));
        letterPositions.Add("x", new Vector3(4.03f, -2.61f, 0f));
        letterPositions.Add("y", new Vector3(5.13f, -2.63f, 0f));
        letterPositions.Add("z", new Vector3(6.37f, -2.83f, 0f));

        letterPositions.Add("yes", new Vector3(-5.84f, 2.39f, 0f));
        letterPositions.Add("no", new Vector3(5.96f, 2.68f, 0f));
    }

    void Start()
    {
        animator.speed = 0;
        InitializeLetterPositions();
        //List<string> test = new List<string> { "k", "n", "k", "n" };
        //SetLetterSequence(test);
    }

    public void StartASequence(string sequenceLetters)
    {
        Debug.Log("Starting a sequence");
        List<string> test = new List<string>();

        if (sequenceLetters == "yes" || sequenceLetters == "no")
        {
            isMovingToYesOrNo = true;
            targetPos = letterPositions[sequenceLetters];
            return;
        }
        else
        {
            foreach (char letter in sequenceLetters)
            {
                test.Add(letter.ToString());
            }
        }
        SetLetterSequence(test);
        animator.speed = 1;
        uiController.canSelectOption = false;
        needToMove = true;
    }

    public void MoveToYesOrNo()
    {
        uiController.canSelectOption = false;
        animator.speed = 1;
        Vector3 direction = (targetPos - transform.position);

        if (direction.magnitude <= 0.05f)
        {
            rigidBody.linearVelocity = Vector2.zero;
            transform.position = targetPos;
            animator.speed = 0;
            isMovingToYesOrNo = false;
            uiController.canSelectOption = true;
            return;
        }

        direction.Normalize();
        rigidBody.linearVelocity = movementSpeed * direction;
    }

    public void SetLetterSequence(List<string> letters)
    {
        foreach (string letter in letters)
        {
            letterSequence.Add(letter);
        }
    }

    private void GetNextLetterInSequence()
    {
        if (letterSequence.Count == 0)
        {
            needToMove = false;
            uiController.canSelectOption = true;
            animator.speed = 0;
            return;
        }

        letterSequence.RemoveAt(0);

        if (letterSequence.Count != 0)
        {
            targetPos = letterPositions[letterSequence[0]];
        }
        else
        {
            needToMove = false;
            uiController.canSelectOption = true;
            animator.speed = 0;
        }
    }

    private void MoveToLetter()
    {
        if (letterSequence.Count == 0)
        {
            needToMove = false;
            uiController.canSelectOption = true;
            animator.speed = 0;
            return;
        }
        targetPos = letterPositions[letterSequence[0]];
        Vector3 direction = (targetPos - transform.position);

        if (direction.magnitude <= 0.05f)
        {
            rigidBody.linearVelocity = Vector2.zero;
            transform.position = targetPos;
            if (!isWaiting)
            {
                isWaiting = true;
                StartCoroutine(LetterWait(1));
            }
            return;
        }

        direction.Normalize();
        rigidBody.linearVelocity = movementSpeed * direction;
    }

    private IEnumerator LetterWait(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetNextLetterInSequence();
        isWaiting = false;
    }

    private void Update()
    {
        if (needToMove)
        {
            MoveToLetter();
        }

        if (isMovingToYesOrNo)
        {
            MoveToYesOrNo();
        }
    }
}
