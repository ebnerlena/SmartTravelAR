public class Question 
{
    public int index { get; private set; }
    public string question { get; private set; }

    public string optionA { get; private set; }

    public string[] options { get; private set; }
    public string optionC { get; private set; }
    public string optionD { get; private set; }

    public char solution { get; private set; }
    public string description { get; private set; }

    public Question(int index, string question, string a, string b, string c, string d, char solution, string description)
    {
        options = new string[4];
        
        this.index = index;
        this.question = question;
        this.options[0] = a;
        this.options[1] = b;
        this.options[2] = c;
        this.options[3] = d;
        this.solution = solution;
        this.description = description;
        
    }

    public bool Answer(char answer)
    {
        return (answer == solution);
    }

    public override string ToString()
    {
        return $"{index}: {question} A: {options[0]} B: {options[1]} C: {options[2]} D: {options[3]}\n Solution: {solution}\n Description: {description}";
    }

    public string GetOptionsText()
    {
        return $"A: {options[0]}\n\nB: {options[1]}\n\nC: {options[2]}\n\nD: {options[3]}";
    }
}