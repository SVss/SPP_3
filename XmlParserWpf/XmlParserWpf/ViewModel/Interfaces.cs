namespace XmlParserWpf.ViewModel
{
    // ITimed

    public interface ITimed
    {
        uint Time { get; set; }
    }

    // IExpandable

    public interface IExpandable
    {
        bool Expanded { get; set; }

        void ExpandAll(object sender);

        void CollapseAll(object sender);
    }

    // IChangeable

    public delegate void ChangeDelegate();

    internal interface IChangeable
    {
        event ChangeDelegate ChangeEvent;

        void OnChange();
    }
}
