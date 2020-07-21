using System.Linq;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


using System;
using System.CodeDom.Compiler;

namespace FB.Utils.JsonPath.Language {



#region COCO/R Generated Code

[GeneratedCode("Coco/R","1.0.0")]
internal class Parser {
	public const int _EOF = 0;
	public const int _stringChar = 1;
	public const int _esc = 2;
	public const int _identQuote = 3;
	public const int _stringQuote = 4;
	public const int _openPar = 5;
	public const int _closePar = 6;
	public const int _true = 7;
	public const int _false = 8;
	public const int _object = 9;
	public const int _array = 10;
	public const int _string = 11;
	public const int _number = 12;
	public const int _boolean = 13;
	public const int _null = 14;
	public const int maxT = 49;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;

	public Scanner scanner;
	public IErrors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public Expression result;
private bool HasColon() {
	var t = la;
	while (t.val != ":" && t.val != "," && t.val != "]" && t.kind!=_EOF) {
		t = scanner.Peek();
	}
	return t.val == ":";
}
private bool LookaheadCheck(string goodToken, params string[] delimitingTokens) {
	var t = la;
	while (!delimitingTokens.Contains(t.val) && t.kind!=_EOF) {
		t = scanner.Peek();
	}
	return t.val == goodToken;
}
public static Expression Parse(string src){
	var errorCollector=new ErrorCollector();
	var p=new Parser(new Scanner(new StringBuffer(src)),errorCollector);
	p.Parse();
	errorCollector.ThrowErrors();
	return p.result;
}

private SourcePosition SP=>new SourcePosition(t.line, t.col);



	public Parser(Scanner scanner, IErrors errors) {
		this.scanner = scanner;
		this.errors = errors??new ErrorCollector();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}

	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}

	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}

	bool StartOf (int s) {
		return set[s, la.kind];
	}

	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}


	void keywords() {
		switch (la.kind) {
		case 9: {
			Get();
			break;
		}
		case 10: {
			Get();
			break;
		}
		case 11: {
			Get();
			break;
		}
		case 12: {
			Get();
			break;
		}
		case 13: {
			Get();
			break;
		}
		case 14: {
			Get();
			break;
		}
		case 7: {
			Get();
			break;
		}
		case 8: {
			Get();
			break;
		}
		case 15: {
			Get();
			break;
		}
		case 16: {
			Get();
			break;
		}
		case 17: {
			Get();
			break;
		}
		case 18: {
			Get();
			break;
		}
		case 19: {
			Get();
			break;
		}
		case 20: {
			Get();
			break;
		}
		case 21: {
			Get();
			break;
		}
		case 22: {
			Get();
			break;
		}
		case 23: {
			Get();
			break;
		}
		case 24: {
			Get();
			break;
		}
		case 25: {
			Get();
			break;
		}
		case 26: {
			Get();
			break;
		}
		default: SynErr(50); break;
		}
	}

	void DigitNonZero(out int n) {
		n=-1; 
		switch (la.kind) {
		case 16: {
			Get();
			n=1; 
			break;
		}
		case 17: {
			Get();
			n=2; 
			break;
		}
		case 18: {
			Get();
			n=3; 
			break;
		}
		case 19: {
			Get();
			n=4; 
			break;
		}
		case 20: {
			Get();
			n=5; 
			break;
		}
		case 21: {
			Get();
			n=6; 
			break;
		}
		case 22: {
			Get();
			n=7; 
			break;
		}
		case 23: {
			Get();
			n=8; 
			break;
		}
		case 24: {
			Get();
			n=9; 
			break;
		}
		default: SynErr(51); break;
		}
	}

	void Digit(out int n) {
		n=0; 
		if (StartOf(1)) {
			DigitNonZero(out n);
		} else if (la.kind == 15) {
			Get();
		} else SynErr(52);
	}

	void SignedInt(out int n) {
		n=0; bool neg=false; 
		if (la.kind == 27) {
			Get();
			neg=true; 
		}
		Digit(out var d);
		n=d; 
		while (StartOf(2)) {
			Digit(out d);
			n=n*10+d; 
		}
		if(neg) n=-n; 
	}

	void Int(out int n) {
		Digit(out var d);
		n=d; 
		while (StartOf(2)) {
			Digit(out d);
			n=n*10+d; 
		}
	}

	void Number(out double f) {
		SignedInt(out var n);
		f=n; 
		if (la.kind == 28) {
			Get();
			Int(out n);
			f=f+(double)n/Math.Pow(10,Math.Ceiling(Math.Log10(n))); 
		}
		if (la.kind == 25 || la.kind == 26) {
			if (la.kind == 25) {
				Get();
			} else {
				Get();
			}
			Int(out n);
			f=f*Math.Pow(10,n); 
		}
	}

	void SignedIntNonZero(out int n) {
		n=0; bool neg=false; 
		if (la.kind == 27) {
			Get();
			neg=true; 
		}
		DigitNonZero(out var d);
		n=d; 
		while (StartOf(2)) {
			Digit(out d);
			n=n*10+d; 
		}
		if(neg) n=-n; 
	}

	void QuotedIdent(out string name) {
		name=""; 
		Expect(3);
		while (StartOf(3)) {
			if (la.kind == 1) {
				Get();
				name+=t.val; 
			} else if (la.kind == 28) {
				Get();
				name+=t.val; 
			} else if (StartOf(4)) {
				keywords();
				name+=t.val; 
			} else if (la.kind == 4) {
				Get();
				name+=t.val; 
			} else if (la.kind == 2) {
				Get();
				Expect(3);
				name+="'"; 
			} else {
				Get();
				Expect(2);
				name+="\\"; 
			}
		}
		Expect(3);
	}

	void Ident(out string name) {
		if (la.kind == 1) {
			Get();
		} else if (StartOf(4)) {
			keywords();
		} else SynErr(53);
		name=t.val; 
		while (StartOf(5)) {
			if (la.kind == 1) {
				Get();
			} else {
				keywords();
			}
			name+=t.val; 
		}
	}

	void QuotedString(out string name) {
		name=""; 
		Expect(4);
		while (StartOf(6)) {
			if (la.kind == 1) {
				Get();
				name+=t.val; 
			} else if (StartOf(2)) {
				Digit(out var nn);
				name+=nn.ToString(); 
			} else if (la.kind == 28) {
				Get();
				name+=t.val; 
			} else if (la.kind == 25) {
				Get();
				name+=t.val; 
			} else if (la.kind == 3) {
				Get();
				name+=t.val; 
			} else if (la.kind == 2) {
				Get();
				Expect(4);
				name+="\""; 
			} else {
				Get();
				Expect(2);
				name+="\\"; 
			}
		}
		Expect(4);
	}

	void Unary(out PredicativeExpressionElement elem) {
		elem=null; 
		switch (la.kind) {
		case 29: {
			Get();
			QualifiedPath(out var pathelem);
			elem=new Reference(SP){Path=pathelem}; 
			break;
		}
		case 15: case 16: case 17: case 18: case 19: case 20: case 21: case 22: case 23: case 24: case 27: {
			Number(out var f);
			elem=new Constant<double>(SP,f); 
			break;
		}
		case 4: {
			QuotedString(out var s);
			elem=new Constant<string>(SP,s); 
			break;
		}
		case 7: {
			Get();
			elem=new Constant<bool>(SP,true); 
			break;
		}
		case 8: {
			Get();
			elem=new Constant<bool>(SP,false); 
			break;
		}
		case 14: {
			Get();
			elem=new NullConstant(SP); 
			break;
		}
		default: SynErr(54); break;
		}
	}

	void QualifiedPath(out PathElement elem) {
		elem=null; 
		if (la.kind == 28) {
			Get();
			RelativePath(out elem);
		} else if (la.kind == 47) {
			Get();
			elem=new DescendantsPathElement(SP); 
			RelativePath(out var innerElem);
			elem.Next=innerElem; 
		} else SynErr(55);
	}

	void Multiplicative(out PredicativeExpressionElement elem) {
		var xelem=new Arithmetic(SP); elem=xelem; 
		Unary(out var d);
		xelem.Left=d; 
		if (la.kind == 30 || la.kind == 31 || la.kind == 32) {
			if (la.kind == 30) {
				Get();
				xelem.Operator=ArithmeticOperator.Multiplication; 
			} else if (la.kind == 31) {
				Get();
				xelem.Operator=ArithmeticOperator.Division; 
			} else {
				Get();
				xelem.Operator=ArithmeticOperator.Modulus; 
			}
			Multiplicative(out d);
			xelem.Right=d; 
		}
	}

	void Additive(out PredicativeExpressionElement elem) {
		var xelem=new Arithmetic(SP); elem=xelem; 
		Multiplicative(out var d);
		xelem.Left=d; 
		if (la.kind == 27 || la.kind == 33) {
			if (la.kind == 33) {
				Get();
				xelem.Operator=ArithmeticOperator.Addition; 
			} else {
				Get();
				xelem.Operator=ArithmeticOperator.Subtraction; 
			}
			Additive(out d);
			xelem.Right=d; 
		}
	}

	void Relational(out PredicativeExpressionElement elem) {
		var xelem=new Relational(SP); elem=xelem; 
		Additive(out var d);
		xelem.Left=d; 
		if (StartOf(7)) {
			if (la.kind == 34) {
				Get();
				xelem.Operator=Relation.Greater; 
			} else if (la.kind == 35) {
				Get();
				xelem.Operator=Relation.GreaterOrEqual; 
			} else if (la.kind == 36) {
				Get();
				xelem.Operator=Relation.Less; 
			} else {
				Get();
				xelem.Operator=Relation.LessOrEqual; 
			}
			Relational(out d);
			xelem.Right=d; 
		}
	}

	void Conjunct(out PredicativeExpressionElement elem) {
		var xelem=new Relational(SP); elem=xelem; 
		Relational(out var d);
		xelem.Left=d; 
		if (la.kind == 38 || la.kind == 39) {
			if (la.kind == 38) {
				Get();
				xelem.Operator=Relation.Equal; 
			} else {
				Get();
				xelem.Operator=Relation.NotEqual; 
			}
			Conjunct(out d);
			xelem.Right=d; 
		}
	}

	void Disjunct(out PredicativeExpressionElement elem) {
		var xelem=new Conjunction(SP); elem=xelem; 
		Conjunct(out var d);
		xelem.Left=d; 
		if (la.kind == 40) {
			Get();
			Disjunct(out d);
			xelem.Right=d; 
		}
	}

	void Disjunction(out PredicativeExpressionElement elem) {
		var xelem=new Disjunction(SP); elem=xelem; 
		Disjunct(out var d);
		xelem.Left=d; 
		if (la.kind == 41) {
			Get();
			Disjunction(out d);
			xelem.Right=d; 
		}
	}

	void Filter(out FilterPredicate filt) {
		Expect(42);
		Disjunction(out var dis);
		filt=new FilterPredicate(SP){Predicate=dis}; 
		Expect(6);
	}

	void UnionExpression(out Predicate pred) {
		pred=null; 
		if (StartOf(8)) {
			RelativePath(out var elem);
			pred=new MemberAccess(SP){Member=elem}; 
		} else if (la.kind == 42) {
			Filter(out var filt);
			pred=filt; 
		} else SynErr(56);
	}

	void RelativePath(out PathElement rv) {
		NodeTest(out var elem);
		rv=elem; 
		if (la.kind == 45) {
			Predicate(out var predicate);
			elem.Predicate=predicate; 
		}
		if (la.kind == 28 || la.kind == 47) {
			QualifiedPath(out var rest);
			elem.Next=rest; 
		}
	}

	void PredicateExpression(out Predicate pred) {
		pred=null; 
		if (la.kind == 30) {
			Get();
			pred=new WildcardPredicate(SP); 
		} else if (HasColon()) {
			var xpred=new ArrayIteration(SP); pred=xpred; 
			if (StartOf(9)) {
				SignedInt(out var n);
				xpred.Start=n; 
			}
			Expect(43);
			if (StartOf(9)) {
				SignedInt(out var n);
				xpred.End=n; 
			}
			if (la.kind == 43) {
				Get();
				SignedIntNonZero(out var n);
				xpred.Step=n; 
			}
		} else if (StartOf(2)) {
			var xpred=new ArrayIndex(SP); pred=xpred; 
			Int(out var n);
			xpred.Add(n); 
			while (la.kind == 44) {
				Get();
				Int(out n);
				xpred.Add(n); 
			}
		} else if (StartOf(10)) {
			var xpred=new UnionPredicate(SP); pred=xpred; 
			UnionExpression(out var p);
			xpred.Items.Add(p); 
			while (la.kind == 44) {
				Get();
				UnionExpression(out p);
				xpred.Items.Add(p); 
			}
		} else SynErr(57);
	}

	void Predicate(out Predicate pred) {
		Expect(45);
		PredicateExpression(out pred);
		Expect(46);
	}

	void Name(out string name) {
		name=null; 
		if (la.kind == 3) {
			QuotedIdent(out name);
		} else if (StartOf(5)) {
			Ident(out name);
		} else SynErr(58);
	}

	void NodeType(out JTokenType? t) {
		t=null; 
		switch (la.kind) {
		case 9: {
			Get();
			Expect(5);
			Expect(6);
			t=JTokenType.Object; 
			break;
		}
		case 10: {
			Get();
			Expect(5);
			Expect(6);
			t=JTokenType.Array; 
			break;
		}
		case 11: {
			Get();
			Expect(5);
			Expect(6);
			t=JTokenType.String; 
			break;
		}
		case 12: {
			Get();
			Expect(5);
			Expect(6);
			t=JTokenType.Float; 
			break;
		}
		case 13: {
			Get();
			Expect(5);
			Expect(6);
			t=JTokenType.Boolean; 
			break;
		}
		case 14: {
			Get();
			Expect(5);
			Expect(6);
			t=JTokenType.Null; 
			break;
		}
		default: SynErr(59); break;
		}
	}

	void NameTest(out string name) {
		name=null; 
		if (la.kind == 30) {
			Get();
		} else if (StartOf(11)) {
			Name(out name);
		} else SynErr(60);
	}

	void NodeTest(out PathElementWithPredicate elem) {
		elem=null; 
		if (StartOf(12)) {
			NodeType(out var t);
			elem=new TypeCheckElement(SP){Type=t.Value}; 
		} else if (StartOf(8)) {
			NameTest(out var name);
			elem=new NameCheckElement(SP){Name=name}; 
		} else SynErr(61);
	}

	void AbsolutePath(out PathElement expr) {
		expr=new RootElement(SP); 
		Expect(48);
		if (la.kind == 28 || la.kind == 47) {
			QualifiedPath(out var items);
			expr.Next=items; 
		}
	}

	void FunctionArgument(out FunctionArgument expr) {
		expr=null; 
		if (StartOf(13)) {
			Expression(out var e);
			expr=new ExpressionArgument(SP){Expression=e}; 
		} else if (StartOf(14)) {
			Disjunction(out var dis);
			expr=new PredicativeArgument(SP){Predicate=dis}; 
		} else SynErr(62);
	}

	void Expression(out Expression expr) {
		expr=null; 
		if (LookaheadCheck("(", "(", ",", ")", "[", ".")) {
			FunctionCall(out var e);
			expr=e; 
		} else if (la.kind == 48) {
			AbsolutePath(out var e);
			expr=e; 
		} else if (StartOf(8)) {
			RelativePath(out var e);
			expr=e; 
		} else SynErr(63);
	}

	void FunctionCall(out FunctionCall expr) {
		expr=new FunctionCall(SP); 
		Name(out var s);
		expr.Function = s; 
		Expect(5);
		if (StartOf(15)) {
			FunctionArgument(out var arg1);
			expr.Arguments.Add(arg1); 
			while (la.kind == 44) {
				Get();
				FunctionArgument(out var argN);
				expr.Arguments.Add(argN); 
			}
		}
		Expect(6);
	}

	void JsonPath() {
		Expression(out var expr);
		result = expr; 
	}



	public void Parse() {
		la = new Token();
		la.val = "";
		Get();
		JsonPath();
		Expect(0);

	}

	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_x, _T,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_x,_T, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_x,_T, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_x,_T, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_x,_T, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x},
		{_x,_x,_x,_x, _T,_x,_x,_T, _T,_x,_x,_x, _x,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_x,_T, _T,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x}

	};
} // end Parser

[GeneratedCode("Coco/R","1.0.0")]
internal class ErrorCollector : IErrors, IEnumerable<Error>
    {
        private readonly List<Error> _errors;

        public void ThrowErrors()
        {
            if(_errors.Count>0)
                throw new InvalidSyntaxException(_errors.ToArray());
        }

        public ErrorCollector()
        {
            _errors = new List<Error>();
        }

        public void SemErr(int line, int col, string s)
        {
            _errors.Add(new Error(line, col, s, ErrorType.Semantic));
        }

        public void SemErr(string s)
        {
            _errors.Add(new Error(-1, -1, s, ErrorType.Semantic));
        }

        public void SynErr(int line, int col, int n)
        {

		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "stringChar expected"; break;
			case 2: s = "esc expected"; break;
			case 3: s = "identQuote expected"; break;
			case 4: s = "stringQuote expected"; break;
			case 5: s = "openPar expected"; break;
			case 6: s = "closePar expected"; break;
			case 7: s = "true expected"; break;
			case 8: s = "false expected"; break;
			case 9: s = "object expected"; break;
			case 10: s = "array expected"; break;
			case 11: s = "string expected"; break;
			case 12: s = "number expected"; break;
			case 13: s = "boolean expected"; break;
			case 14: s = "null expected"; break;
			case 15: s = "\"0\" expected"; break;
			case 16: s = "\"1\" expected"; break;
			case 17: s = "\"2\" expected"; break;
			case 18: s = "\"3\" expected"; break;
			case 19: s = "\"4\" expected"; break;
			case 20: s = "\"5\" expected"; break;
			case 21: s = "\"6\" expected"; break;
			case 22: s = "\"7\" expected"; break;
			case 23: s = "\"8\" expected"; break;
			case 24: s = "\"9\" expected"; break;
			case 25: s = "\"e\" expected"; break;
			case 26: s = "\"E\" expected"; break;
			case 27: s = "\"-\" expected"; break;
			case 28: s = "\".\" expected"; break;
			case 29: s = "\"@\" expected"; break;
			case 30: s = "\"*\" expected"; break;
			case 31: s = "\"/\" expected"; break;
			case 32: s = "\"%\" expected"; break;
			case 33: s = "\"+\" expected"; break;
			case 34: s = "\">\" expected"; break;
			case 35: s = "\">=\" expected"; break;
			case 36: s = "\"<\" expected"; break;
			case 37: s = "\"<=\" expected"; break;
			case 38: s = "\"=\" expected"; break;
			case 39: s = "\"!=\" expected"; break;
			case 40: s = "\"&&\" expected"; break;
			case 41: s = "\"||\" expected"; break;
			case 42: s = "\"?(\" expected"; break;
			case 43: s = "\":\" expected"; break;
			case 44: s = "\",\" expected"; break;
			case 45: s = "\"[\" expected"; break;
			case 46: s = "\"]\" expected"; break;
			case 47: s = "\"..\" expected"; break;
			case 48: s = "\"$\" expected"; break;
			case 49: s = "??? expected"; break;
			case 50: s = "invalid keywords"; break;
			case 51: s = "invalid DigitNonZero"; break;
			case 52: s = "invalid Digit"; break;
			case 53: s = "invalid Ident"; break;
			case 54: s = "invalid Unary"; break;
			case 55: s = "invalid QualifiedPath"; break;
			case 56: s = "invalid UnionExpression"; break;
			case 57: s = "invalid PredicateExpression"; break;
			case 58: s = "invalid Name"; break;
			case 59: s = "invalid NodeType"; break;
			case 60: s = "invalid NameTest"; break;
			case 61: s = "invalid NodeTest"; break;
			case 62: s = "invalid FunctionArgument"; break;
			case 63: s = "invalid Expression"; break;

			default: s = "error " + n; break;
		}
            _errors.Add(new Error(line, col, s, ErrorType.Syntax));
        }

        public void Warning(int line, int col, string s)
        {
            _errors.Add(new Error(line, col, s, ErrorType.Syntax));
        }

        public void Warning(string s)
        {
            _errors.Add(new Error(-1, -1, s, ErrorType.Syntax));
        }

        public int Count => _errors.Count;

        public IEnumerator<Error> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

[GeneratedCode("Coco/R","1.0.0")]
internal class FatalError: Exception {
	public FatalError(string m): base(m) {}
}

#endregion
}