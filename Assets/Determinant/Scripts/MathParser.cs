using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class MathParser : MonoBehaviour {
	/*TODO:
	 *create exception class and select offending symbols
	 * ^ symbol doesn't work!
	 */
	//CLASSES
	private abstract class Token { //holds text, as well as preceding and following tokens
		public string text;
		private Token _prev;
		public Token prev { //this is basically a linked list
			get {return _prev;}
			set {_prev = value;
				if(value != null) value._next = this;
			}
		}
		private Token _next;
		public Token next {
			get {return _next;}
			set {
				_next = value;
				if (value != null) value._prev = this;
			}
		}
	}
	private class Operator : Token { // -, +, *, /, or ^
		public Func<float, float, float> calculate;
		public int precedence;
		public Operator(Func<float, float, float> operation, int precedence) {
			this.calculate = operation;
			this.precedence = precedence;
		}
	}
	private class Number : Token {
		public float value;
		public Number(string text) {
			this.text = text;
			this.value = float.Parse(text);
		}
	}
	private class OpenParen : Token {
		public OpenParen() { }
	}
	private class CloseParen : Token {
		public CloseParen() { }
	}
	private class CalculatorException : Exception {
		public CalculatorException(int position, int length) {
		}
	}

	//VARIABLES
	private readonly char[] symbols = {'-', '+', '*', '/', '^'}; //used to identify operators in input
	private readonly Func<float, float, float>[] functions = {(x, y) => x-y, (x, y) => x+y, (x, y) => x*y, (x, y) => x/y, (x, y) => Mathf.Pow(x, y)}; //used to assign operators a corresponding operation
	private InputField inputField;
	private float calculationDelay = 0;//WaitForSeconds parameter, used to offset each calcultion by fixed time

	//METHODS
	void Start() {
		inputField = GetComponent<InputField>();
		inputField.onEndEdit.AddListener((value) => calculate(value)); // Add listener to catch the submit (when set Submit button is pressed)
	}

	//calls all preceding functions
	public void calculate(string text) {
		Token head = null;
		try {
			head = tokenize(text);
			checkSyntax(head);
		} catch(CalculatorException e) {
			Debug.Log("ERROR!");
			//highlight the character in red
		}
		parse(head);
	}

	//converts a math string into a series of Tokens
	private Token tokenize(string input) {
		Token head = new OpenParen(); //will be returned at end of function
		StringBuilder spaceHolder = new StringBuilder(); //holds preceding and trailing whitespace
		Token last = head; //beginning of string behaves like an openParen

		for(int i = 0; i < input.Length; i++) { //tokenize each character of input
			Type lastType = last.GetType();
			//case: character is whitespace
			if(char.IsWhiteSpace(input, i)) {
				spaceHolder.Append(input[i]); //an operator includes the whitespace on its sides
				continue;
			}
			//case: character is part of a number
			Match match = new Regex(@"\G(\d*\.)?\d+").Match(input, i); //matches any positive float
			bool matchFound = match.Success; //true if pos or neg float is found
			bool isNegative = false; //true if float is negative, but not positive
			if(!matchFound && (lastType == typeof(Operator) || lastType == typeof(OpenParen))) { //negative numbers are only allowed after operators or open brackets
				match = new Regex(@"\G-(\d*\.)?\d+").Match(input, i); //matches any negative float
				matchFound = isNegative = match.Success;
			}
			if(matchFound) {
				Debug.Log("Number: " + match.Value);
				if(!isNegative && lastType == typeof(CloseParen)) { //numbers after brackets imply multiplication
					last.next = new Operator((x, y) => x*y, 2); //add * operator to output
					last = last.next; //update last
				}
				i += match.Length-1; //skip to the position at the end of the float
				last.text = spaceHolder.ToString(); //add preceding whitespace to operator
				spaceHolder.Length = 0; //trailing whitespace belongs to next operator
				last.next = new Number(match.Value); //add Number to output
				last = last.next; //update last
				continue;
			}
			//case: character is an operator
			int precedence = 0;
			for(;precedence < symbols.Length && input[i] != symbols[precedence]; precedence++);
			if(precedence < symbols.Length) { //if character is operator
				spaceHolder.Append(input[i]);
				last.next = new Operator(functions[precedence], precedence);
				last = last.next; //update last
				Debug.Log("Op: " + input[i]);
				continue;
			}
			//case: character is open bracket
			if (input[i] == '(') {
				if(lastType == typeof(Number) || lastType == typeof(CloseParen)) { //these imply multiplication 
					last.next = new Operator((x, y) => x*y, 2); //add an invisible times operator to output
					last = last.next; //update last
				}
				last.text = spaceHolder.ToString(); //append preceding whitespace to last operator
				spaceHolder.Length = 0;
				last.next = new OpenParen();
				spaceHolder.Append('('); //trailing whitespace belongs to this OpenParen
				last = last.next; //update last
				continue;
			}
			//case: character is close bracket
			if (input[i] == ')') {
				last.next = new CloseParen();
				spaceHolder.Append(')');
				last.text = spaceHolder.ToString(); //append preceding whitespace to this CloseParen
				spaceHolder.Length = 0;
				last = last.next; //update last
				continue;
			}
			//case: character did not match any case
			Debug.Log("Unrecognized Char: " + input[i]);
		}
		head = head.next; //remove unnecessary head
		head.prev = null;
		return head;
	}

	//throws CalculatorException if any incompatible tokens are next to each other
	private void checkSyntax(Token head) {
		int bracketDepth = 0; //total OpenParens minus total CloseParens
		//first character must be Number or OpenParen
		for(Token token = head; token.next != null; token = token.next) {
			Type type = token.GetType();
			Type nextType = token.next.GetType();
			if(type == typeof(Number)) { //number must be followed by operator or close bracket
				if(nextType != typeof(Operator) && nextType != typeof(CloseParen)) {
					//throw error
				}
			} else if(type == typeof(Operator)) { //operator must be followed by number or open bracket
				if(nextType != typeof(Number) && nextType != typeof(OpenParen)) {
					//throw error
				}
			} else if(type == typeof(OpenParen)) { //open bracket must be followed by number or bracket
				bracketDepth++;
				if(nextType == typeof(Operator)) {
					//throw error
				}
			} else if(type == typeof(CloseParen)) { //close bracket must be followed by operator or close bracket
				bracketDepth--;
				if(nextType != typeof(Operator) && nextType != typeof(CloseParen)) {
					//throw error
				}
			}
		}
		//last character must be Number or CloseParen
		if(bracketDepth != 0) { //brackets must be balanced
			//throw error
		}
		calculationDelay = 0;//so that accumulated delay isn't left over from last parse was called
		Debug.Log("syntax check success");
	}

	//BODMAS math parser. Handles brackets by recursion.
	private void parse(Token head) {
		Queue<Operator>[] queues = new Queue<Operator>[symbols.Length];
		for(int i = 0; i < symbols.Length; i++) { //initialize operator queues
			queues[i] = new Queue<Operator>();
		}
		Stack<Operator> powerStack = new Stack<Operator>(); //stores ^ operators to handle right-associativity

		print(head); //shows removal of leading & trailing whitespace
		for(Token token = head; token != null; token = token.next) {
			if(token is Operator) { //push operator to its respective queue
				Operator op = token as Operator;
				if(op.precedence == 4) { // if operator is ^
					powerStack.Push(op); //store in stack
				} else { //operator is not ^
					for(int i = powerStack.Count; i > 0; i--) { //if powerStack is not empty, pop all to queue
						queues[4].Enqueue(powerStack.Pop());
					}
					queues[op.precedence].Enqueue(op); //enqueue current operator
				}
			} else if(token is OpenParen) {
				parse(token.next); //recurse
				token.prev.next = token.next;
			} else if(token is CloseParen) {
				popAll(queues, head); //pop all queues
				token.prev.next = token.next; //delete CloseParen
				return; //recurse finished
			}
		}
		popAll(queues, head);
		Debug.Log("parse success");
	}
	//helper for parse function, empties all queues and executes their contents
	private void popAll(Queue<Operator>[] queues, Token head) {
		for(int i = queues.Length-1; i >= 0; i--) { //pop high to low precedence
			Debug.Log("popAll, i="+i);
			for(int j = queues[i].Count; j > 0; j--) {
				Debug.Log("popAll, j="+j);
				Operator op = queues[i].Dequeue();
				Number num1 = op.prev as Number; //get adjacent number
				Number num2 = op.next as Number; //get adjacent number
				num1.value = op.calculate(num1.value, num2.value); //store result in num1
				num1.text = string.Format("{0:#,0.#}", num1.value); //value to 2 decimal places
				num1.next = num2.next; //delete op and num2
				StartCoroutine(print(head)); //StartCoroutine is need to delay calcuating each answer
			}
		}
	}
		
	//called after every math operation to display the updated result. Waits 0.5 per operation.
	private IEnumerator print(Token head) {
		StringBuilder output = new StringBuilder();
		for(Token token = head; token != null; token = token.next) {
			output.Append(token.text);
		}
		calculationDelay += 0.75f; //elapsed time between each calculation is dislplayed on screen
		Debug.Log ("CalculationDelay: " + calculationDelay);
		yield return new WaitForSeconds(calculationDelay); //saves and returns this function as type IEnumerator to be executed later 
		inputField.text = output.ToString();
		Debug.Log("Print: " + output.ToString());

	}
}