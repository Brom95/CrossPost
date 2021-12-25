namespace Message;
public struct Message
{
    public string Text { get; set; }
    public byte[] Image { get; set; }

    public byte[] Attachment { get; set; }
}
