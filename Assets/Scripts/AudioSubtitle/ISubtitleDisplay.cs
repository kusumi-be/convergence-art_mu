namespace AudioSubtitle
{
    /// <summary>
    /// 字幕表示のインターフェース
    /// </summary>
    public interface ISubtitleDisplay
    {
        /// <summary>
        /// 字幕を表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        void ShowSubtitle(string text);

        /// <summary>
        /// 字幕を非表示にする
        /// </summary>
        void HideSubtitle();

        /// <summary>
        /// 字幕をクリアする
        /// </summary>
        void ClearSubtitle();
    }
}
