
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.CodeDom.Compiler;

namespace FB.Utils.JsonPath.Language {
#region COCO/R Generated Code

[GeneratedCode("Coco/R","1.0.0")]
internal class Token {
	public int kind;    // token kind
	public int pos;     // token position in bytes in the source text (starting at 0)
	public int charPos;  // token position in characters in the source text (starting at 0)
	public int col;     // token column (starting at 1)
	public int line;    // token line (starting at 1)
	public string val;  // token value
	public Token next;  // ML 2005-03-11 Tokens are kept in linked list
}

//-----------------------------------------------------------------------------------
// Buffer
//-----------------------------------------------------------------------------------
[GeneratedCode("Coco/R","1.0.0")]
internal interface IBuffer{
  int Read(); 
  int Peek();
  int Pos{get;set;}
  string GetString(int start, int end);
}
[GeneratedCode("Coco/R","1.0.0")]
static class Buffer{
  public const int EOF = char.MaxValue + 1;
}
//-----------------------------------------------------------------------------------
// Scanner
//-----------------------------------------------------------------------------------
[GeneratedCode("Coco/R","1.0.0")]
internal class Scanner {
	const char EOL = '\n';
	const int eofSym = 0; /* pdt */
	const int maxT = 49;
	const int noSym = 49;


	public IBuffer buffer; // scanner buffer
	
	Token t;          // current token
	int ch;           // current input character
	int pos;          // byte position of current character
	int charPos;      // position by unicode characters starting with 0
	int col;          // column number of current character
	int line;         // line number of current character
	int oldEols;      // EOLs that appeared in a comment;
	static readonly Dictionary<int,int> start; // maps first token character to start state

	Token tokens;     // list of tokens already peeked (first token is a dummy)
	Token pt;         // current peek token
	
	char[] tval = new char[128]; // text of current token
	int tlen;         // length of current token
	
	static Scanner() {
		start = new Dictionary<int,int>(128);
		for (int i = 0; i <= 32; ++i) start[i] = 1;
		for (int i = 35; i <= 37; ++i) start[i] = 1;
		for (int i = 40; i <= 45; ++i) start[i] = 1;
		for (int i = 47; i <= 59; ++i) start[i] = 1;
		for (int i = 61; i <= 61; ++i) start[i] = 1;
		for (int i = 64; i <= 91; ++i) start[i] = 1;
		for (int i = 93; i <= 96; ++i) start[i] = 1;
		for (int i = 99; i <= 101; ++i) start[i] = 1;
		for (int i = 103; i <= 109; ++i) start[i] = 1;
		for (int i = 112; i <= 114; ++i) start[i] = 1;
		for (int i = 117; i <= 123; ++i) start[i] = 1;
		for (int i = 125; i <= 65535; ++i) start[i] = 1;
		for (int i = 92; i <= 92; ++i) start[i] = 2;
		for (int i = 39; i <= 39; ++i) start[i] = 3;
		for (int i = 34; i <= 34; ++i) start[i] = 4;
		start[116] = 38; 
		start[102] = 39; 
		start[111] = 40; 
		start[97] = 41; 
		start[115] = 42; 
		start[110] = 43; 
		start[98] = 44; 
		start[46] = 53; 
		start[62] = 54; 
		start[60] = 55; 
		start[33] = 56; 
		start[38] = 57; 
		start[124] = 58; 
		start[63] = 59; 
		start[Buffer.EOF] = -1;

	}	
	
	public Scanner (IBuffer buffer) {
		this.buffer = buffer;
		Init();
	}
	
	void Init() {
		pos = -1; line = 1; col = 0; charPos = -1;
		oldEols = 0;
		NextCh();
		pt = tokens = new Token();  // first token is a dummy
	}
	
	void NextCh() {
		if (oldEols > 0) { ch = EOL; oldEols--; } 
		else {
			pos = buffer.Pos;
			// buffer reads unicode chars, if UTF8 has been detected
			ch = buffer.Read(); col++; charPos++;
			// replace isolated '\r' by '\n' in order to make
			// eol handling uniform across Windows, Unix and Mac
			if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
			if (ch == EOL) { line++; col = 0; }
		}

	}

	void AddCh() {
		if (tlen >= tval.Length) {
			char[] newBuf = new char[2 * tval.Length];
			Array.Copy(tval, 0, newBuf, 0, tval.Length);
			tval = newBuf;
		}
		if (ch != Buffer.EOF) {
			tval[tlen++] = (char) ch;
			NextCh();
		}
	}




	void CheckLiteral() {
		switch (t.val) {
			case "(": t.kind = 5; break;
			case ")": t.kind = 6; break;
			case "0": t.kind = 15; break;
			case "1": t.kind = 16; break;
			case "2": t.kind = 17; break;
			case "3": t.kind = 18; break;
			case "4": t.kind = 19; break;
			case "5": t.kind = 20; break;
			case "6": t.kind = 21; break;
			case "7": t.kind = 22; break;
			case "8": t.kind = 23; break;
			case "9": t.kind = 24; break;
			case "e": t.kind = 25; break;
			case "E": t.kind = 26; break;
			case "-": t.kind = 27; break;
			case "@": t.kind = 29; break;
			case "*": t.kind = 30; break;
			case "/": t.kind = 31; break;
			case "%": t.kind = 32; break;
			case "+": t.kind = 33; break;
			case ">": t.kind = 34; break;
			case "<": t.kind = 36; break;
			case "=": t.kind = 38; break;
			case ":": t.kind = 43; break;
			case ",": t.kind = 44; break;
			case "[": t.kind = 45; break;
			case "]": t.kind = 46; break;
			case "$": t.kind = 48; break;
			default: break;
		}
	}

	Token NextToken() {
		while (ch == ' ' ||
			ch >= 9 && ch <= 10 || ch == 13
		) NextCh();

		int recKind = noSym;
		int recEnd = pos;
		t = new Token();
		t.pos = pos; t.col = col; t.line = line; t.charPos = charPos;
		int state;
		if (start.ContainsKey(ch)) { state = (int) start[ch]; }
		else { state = 0; }
		tlen = 0; AddCh();
		
		switch (state) {
			case -1: { t.kind = eofSym; break; } // NextCh already done
			case 0: {
				if (recKind != noSym) {
					tlen = recEnd - t.pos;
					SetScannerBehindT();
				}
				t.kind = recKind; break;
			} // NextCh already done
			case 1:
				{t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 2:
				{t.kind = 2; break;}
			case 3:
				{t.kind = 3; break;}
			case 4:
				{t.kind = 4; break;}
			case 5:
				if (ch == 'u') {AddCh(); goto case 6;}
				else {goto case 0;}
			case 6:
				if (ch == 'e') {AddCh(); goto case 7;}
				else {goto case 0;}
			case 7:
				{t.kind = 7; break;}
			case 8:
				if (ch == 'l') {AddCh(); goto case 9;}
				else {goto case 0;}
			case 9:
				if (ch == 's') {AddCh(); goto case 10;}
				else {goto case 0;}
			case 10:
				if (ch == 'e') {AddCh(); goto case 11;}
				else {goto case 0;}
			case 11:
				{t.kind = 8; break;}
			case 12:
				if (ch == 'j') {AddCh(); goto case 13;}
				else {goto case 0;}
			case 13:
				if (ch == 'e') {AddCh(); goto case 14;}
				else {goto case 0;}
			case 14:
				if (ch == 'c') {AddCh(); goto case 15;}
				else {goto case 0;}
			case 15:
				if (ch == 't') {AddCh(); goto case 16;}
				else {goto case 0;}
			case 16:
				{t.kind = 9; break;}
			case 17:
				if (ch == 'r') {AddCh(); goto case 18;}
				else {goto case 0;}
			case 18:
				if (ch == 'a') {AddCh(); goto case 19;}
				else {goto case 0;}
			case 19:
				if (ch == 'y') {AddCh(); goto case 20;}
				else {goto case 0;}
			case 20:
				{t.kind = 10; break;}
			case 21:
				if (ch == 'r') {AddCh(); goto case 22;}
				else {goto case 0;}
			case 22:
				if (ch == 'i') {AddCh(); goto case 23;}
				else {goto case 0;}
			case 23:
				if (ch == 'n') {AddCh(); goto case 24;}
				else {goto case 0;}
			case 24:
				if (ch == 'g') {AddCh(); goto case 25;}
				else {goto case 0;}
			case 25:
				{t.kind = 11; break;}
			case 26:
				if (ch == 'b') {AddCh(); goto case 27;}
				else {goto case 0;}
			case 27:
				if (ch == 'e') {AddCh(); goto case 28;}
				else {goto case 0;}
			case 28:
				if (ch == 'r') {AddCh(); goto case 29;}
				else {goto case 0;}
			case 29:
				{t.kind = 12; break;}
			case 30:
				if (ch == 'o') {AddCh(); goto case 31;}
				else {goto case 0;}
			case 31:
				if (ch == 'l') {AddCh(); goto case 32;}
				else {goto case 0;}
			case 32:
				if (ch == 'e') {AddCh(); goto case 33;}
				else {goto case 0;}
			case 33:
				if (ch == 'a') {AddCh(); goto case 34;}
				else {goto case 0;}
			case 34:
				if (ch == 'n') {AddCh(); goto case 35;}
				else {goto case 0;}
			case 35:
				{t.kind = 13; break;}
			case 36:
				if (ch == 'l') {AddCh(); goto case 37;}
				else {goto case 0;}
			case 37:
				{t.kind = 14; break;}
			case 38:
				recEnd = pos; recKind = 1;
				if (ch == 'r') {AddCh(); goto case 5;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 39:
				recEnd = pos; recKind = 1;
				if (ch == 'a') {AddCh(); goto case 8;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 40:
				recEnd = pos; recKind = 1;
				if (ch == 'b') {AddCh(); goto case 12;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 41:
				recEnd = pos; recKind = 1;
				if (ch == 'r') {AddCh(); goto case 17;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 42:
				recEnd = pos; recKind = 1;
				if (ch == 't') {AddCh(); goto case 21;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 43:
				recEnd = pos; recKind = 1;
				if (ch == 'u') {AddCh(); goto case 45;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 44:
				recEnd = pos; recKind = 1;
				if (ch == 'o') {AddCh(); goto case 30;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 45:
				if (ch == 'm') {AddCh(); goto case 26;}
				else if (ch == 'l') {AddCh(); goto case 36;}
				else {goto case 0;}
			case 46:
				{t.kind = 35; break;}
			case 47:
				{t.kind = 37; break;}
			case 48:
				{t.kind = 39; break;}
			case 49:
				{t.kind = 40; break;}
			case 50:
				{t.kind = 41; break;}
			case 51:
				{t.kind = 42; break;}
			case 52:
				{t.kind = 47; break;}
			case 53:
				recEnd = pos; recKind = 28;
				if (ch == '.') {AddCh(); goto case 52;}
				else {t.kind = 28; break;}
			case 54:
				recEnd = pos; recKind = 1;
				if (ch == '=') {AddCh(); goto case 46;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 55:
				recEnd = pos; recKind = 1;
				if (ch == '=') {AddCh(); goto case 47;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 56:
				recEnd = pos; recKind = 1;
				if (ch == '=') {AddCh(); goto case 48;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 57:
				recEnd = pos; recKind = 1;
				if (ch == '&') {AddCh(); goto case 49;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 58:
				recEnd = pos; recKind = 1;
				if (ch == '|') {AddCh(); goto case 50;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 59:
				recEnd = pos; recKind = 1;
				if (ch == '(') {AddCh(); goto case 51;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}

		}
		t.val = new String(tval, 0, tlen);
		return t;
	}
	
	private void SetScannerBehindT() {
		buffer.Pos = t.pos;
		NextCh();
		line = t.line; col = t.col; charPos = t.charPos;
		for (int i = 0; i < tlen; i++) NextCh();
	}
	
	// get the next token (possibly a token already seen during peeking)
	public Token Scan () {
		if (tokens.next == null) {
			return NextToken();
		} else {
			pt = tokens = tokens.next;
			return tokens;
		}
	}

	// peek for the next token, ignore pragmas
	public Token Peek () {
		do {
			if (pt.next == null) {
				pt.next = NextToken();
			}
			pt = pt.next;
		} while (pt.kind > maxT); // skip pragmas
	
		return pt;
	}

	// make sure that peeking starts at the current scan position
	public void ResetPeek () { pt = tokens; }

} // end Scanner
#endregion
}