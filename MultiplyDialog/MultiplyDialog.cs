using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

public class MultiplyDialog : Dialog
{
    [JsonConstructor]
    public MultiplyDialog([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        : base()
    {
        // enable instances of this command as debug break point
        RegisterSourceLocation(sourceFilePath, sourceLineNumber);
    }

    [JsonProperty("$kind")]
    public const string Kind = "MultiplyDialog";

    [JsonProperty("arg1")]
    public NumberExpression Arg1 { get; set; }

    [JsonProperty("arg2")]
    public NumberExpression Arg2 { get; set; }

    [JsonProperty("resultProperty")]
    public StringExpression ResultProperty { get; set; }

    public override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var arg1 = Arg1.GetValue(dc.State);
        var arg2 = Arg2.GetValue(dc.State);

        var result = Convert.ToInt32(arg1) * Convert.ToInt32(arg2);
        if (this.ResultProperty != null)
        {
            dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);
        }

        return dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
    }
}