using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace projekt1
{
    public class RPN
    {
        string rownanie;
        double x, min, max;
        int step, minus;
        List<string> L = new List<string>();
        Stack<string> S = new Stack<string>();
        Queue<string> Q = new Queue<string>();

        public RPN(string rownanie, double x, double xMin, double xMax, int n)
        {
            this.rownanie = rownanie;
            this.x = x;
            this.min = xMin;
            this.max = xMax;
            this.step = n;
        }

        public string[] Tokeny()
        {
            Regex reg = new Regex(@"\+|\-|\*|\/|\(|\)|\^|(x)|(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan)|((\d*)(\.)?(\d+))");
            MatchCollection token = reg.Matches(this.rownanie);
            string[] tokeny = new string[token.Count];
            int i = 0;

            foreach (Match tok in token)
            {
                tokeny[i] = tok.Value;
                i++;
            }

            if (this.rownanie[0] == '-' && this.rownanie[1] == '(')
            {
                this.minus = 1;
                this.rownanie = this.rownanie.Remove(0, 1);
            }
            else if (this.rownanie[0] == '-' && this.rownanie[1] != '(')
            {
                this.rownanie = "0" + this.rownanie;
            }
            return tokeny;
        }
        static Dictionary<string, int> D = new Dictionary<string, int>()
        {
            {"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},
            {"tan",4},{"cosh",4},{"sinh",4},{"tanh",4},{"acos",4},{"asin",4},{"atan",4},
            {"^",3},
            {"*",2},{"/",2},
            {"+",1},{"-",1},
            {"(",0}
        };

        public void walidacja()
        {
            int nawias = 0;
            int i = 0;
            string[] Token = new string[this.Tokeny().Length];
            Token = this.Tokeny();
            
            foreach(string token in this.Tokeny())
            { 

                    if (token == "(")
                    {
                        nawias++;
                    }
                    else if (token == ")")
                    {
                        nawias--;
                    }

            }
            if (nawias != 0)
            {
                Console.WriteLine("Błąd:nie zamnięty nawias");
                Environment.Exit(0);
            }
            for (i = 0; i < Token.Length; i++)
            {
                if (D.ContainsKey(Token[i]))
                {
                    if (D.ContainsKey(Token[i + 1]) && (D[Token[i]] == 1 || D[Token[i]] == 2 || D[Token[i]] == 3) && (D[Token[i + 1]] == 1 || D[Token[i + 1]] == 2 || D[Token[i + 1]] == 3))
                    {
                        Console.WriteLine("Błędne wyrażenie: operatory są obok siebie");
                        Environment.Exit(0);
                    }

                }
            }
            foreach (string t in Token)
            {
                Console.Write("{0}", t);
            }
        }

        public void Postfix()
        {
            double temp;

            foreach (string token in this.Tokeny())
            {
                if (token == "(")
                {
                    S.Push(token);
                }
                else if (token == ")")
                {
                    while (S.Peek() != "(")
                    {
                        Q.Enqueue(S.Pop());
                        S.Pop();
                    }
                }
                else if (D.ContainsKey(token))
                {
                    while (S.Count > 0 && D[token] <= D[S.Peek()])
                    {
                        Q.Enqueue(S.Pop());
                        S.Push(token);
                    }
                }
                else if (Double.TryParse(token, out temp) || token == "x")
                {
                    Q.Enqueue(token);
                }
            }
            while (S.Count > 0)
            {
                Q.Enqueue(S.Pop());
            }
            foreach (string temp1 in Q.ToArray())
            {
                L.Add(temp1);
                Console.WriteLine("0 ", temp1);
            }
            Console.WriteLine();
        }
        public double oblicz()
        {
            double temp2;
            Stack<double> S1 = new Stack<double>();

            foreach (string token in L)
            {
                if (Double.TryParse(token, out temp2))
                {
                    S1.Push(temp2);
                }
                else if (token == "x")
                {
                    S1.Push(this.x);
                }
                else if (D.ContainsKey(token))
                {
                    double a = S1.Pop();
                    if (D[token]==4)
                    {
                        if (token == "abs")
                        {
                            a = Math.Abs(a);
                        }
                        else if (token == "cos")
                        {
                            a = Math.Cos(a);
                        }
                        else if (token == "exp")
                        {
                            a = Math.Exp(a);
                        }
                        else if (token == "log")
                        {
                            a = Math.Log(a);
                        }
                        else if (token == "sin")
                        {
                            a = Math.Sin(a);
                        }
                        else if (token == "sqrt")
                        {
                            a = Math.Sqrt(a);
                        }
                        else if (token == "tan")
                        {
                            a = Math.Tan(a);
                        }
                        else if (token == "cosh")
                        {
                            a = Math.Cosh(a);
                        }
                        else if (token == "sinh")
                        {
                            a = Math.Sinh(a);
                        }
                        else if (token == "tanh")
                        {
                            a = Math.Tanh(a);
                        }
                        else if (token == "acos")
                        {
                            a = Math.Acos(a);
                        }
                        else if (token == "asin")
                        {
                            a = Math.Asin(a);
                        }
                        else if (token == "atan")
                        {
                            a = Math.Atan(a);
                        }
                    }
                    else
                    {
                        double b = S1.Pop();
                        if (token == "+")
                        {
                            a = a + b;
                        }
                        else if (token == "-")
                        {
                            a = a - b;
                        }
                        else if (token == "*")
                        {
                            a = a * b;
                        }
                        else if (token == "^")
                        {
                            Math.Pow(b, a);
                        }
                        else if (token == "/")
                        {
                            if (a == 0)
                            {
                                Console.WriteLine("Bład: Nie dziel cholero przez zero");
                                Environment.Exit(0);
                            }
                            else
                            {
                                a = b / a;
                            }
                        }
                    }

                    S1.Push(a);
                }
            }
            double wynik = S1.Pop();

            if (this.minus == 1)
            {
                wynik = wynik * (-1);
            }

            return wynik;
        }
        public void Przedzial()
        {
            double przedzial = (this.max - this.min) / (this.step - 1);
            this.x = this.min;

            for (int i = 0; i < this.step; i++)
            {
                Console.WriteLine("{0} => {1}", this.x, this.oblicz());
                this.x += przedzial;
            }
        }
    }
}
