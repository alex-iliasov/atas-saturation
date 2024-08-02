using System;
using System.ComponentModel;
using System.Windows.Media;
using ATAS.Indicators;
using System.ComponentModel.DataAnnotations;


//namespace SaturationIndicator
//{
//    [Category("!Trading & Psychology")]
//    [DisplayName("Насыщенность")]
//    [Description("ОИ / Delta")]
//    public class Saturation : Indicator
//    {
//        #region Настройки

//        [Display(GroupName = "Основные настройки", Order = 0, Name = "Процент насыщенности")]
//        public decimal SaturationPercentage { get; set; } = 8;


//        [Display(GroupName = "Основные настройки", Order = 0, Name = "Цвет положительной дельты")]
//        public Color PositiveDeltaColor { get; set; } = Colors.Blue;


//        [Display(GroupName = "Основные настройки", Order = 0, Name = "Цвет отрицательной дельты")]
//        public Color NegativeDeltaColor { get; set; } = Colors.Pink;

//        #endregion

//        private readonly PaintbarsDataSeries _colorSeries = new PaintbarsDataSeries("Закраска бара")
//        {
//            IsHidden = true,
//            Visible = true,
//            DrawAbovePrice = true,
//        };

//        public Saturation()
//        {
//            //DataSeries.Add(_volumeSeries);
//            //DataSeries.Add(_deltaSeries);
//            DataSeries.Add(_colorSeries);
//        }

//        protected override void OnCalculate(int bar, decimal value)
//        {
//            var delta = GetDelta(bar);
//            var volume = GetVolume(bar);

//            if (volume == 0)
//                return;

//            var saturation = Math.Abs(delta / volume) * 100;

//            if (saturation >= SaturationPercentage)
//            {
//                _colorSeries[bar] = delta > 0 ? PositiveDeltaColor : NegativeDeltaColor;
//            }
//        }

//        private decimal GetDelta(int bar)
//        {
//            // Логика получения дельты для текущего бара
//            return GetCandle(bar).Delta;
//        }

//        private decimal GetVolume(int bar)
//        {
//            // Логика получения объема для текущего бара
//            return GetCandle(bar).Volume; 
//        }
//    }
//}

namespace VDOWithSaturation
{
    [Category("!Trading & Psychology")]
    [DisplayName("VDO Constructor with Saturation 1.1.0")]
    [Description("Универсальный конструктор усилий")]

    public class VDOWithSaturation : Indicator
    {
       
        #region Настройка. Основные настройки #0
        private bool _OnOff;
        [Display(GroupName = "Основные настройки", Order = 0, Name = "Вкл. Индикатор")]
        public bool OnOff { get => _OnOff; set { _OnOff = value; RecalculateValues(); } }

        private string _NameConstructor;
        [Display(GroupName = "Основные настройки", Order = 1, Name = "Имя",  Description = "Имя конструктора (Покупатель, Продавец и т.д.). Используется в Алертах")]
        public string NameConstructor { get => _NameConstructor; set { _NameConstructor = value; RecalculateValues(); } }
        #endregion

        #region Настройка. Оповещение #10
        //checkbox для алерта
        private bool _UseAlert;
        [Display(GroupName = "Оповещение", Order = 10, Name = "Вкл. Алерт")]
        public bool UseAlert { get => _UseAlert; set { _UseAlert = value; RecalculateValues(); } }
        /*
        //checkbox для алерта за N минут
        private bool _UseAlertN;
        [Display(GroupName = "Оповещение", Order = 11, Name = "Вкл. Ранний Алерт", Description = "Заблаговременное оповещение за выбранное количество минут до закрытия бара")]
        public bool UseAlertN { get => _UseAlertN; set { _UseAlertN = value; RecalculateValues(); } }

        private int _NforAlert = 5;
        [Display(GroupName = "Оповещение", Order = 12, Name = "Настройка минут для Раннего Алерта", Description = "Количество минут до закрытия бара, когда сработает Ранний Алерт")]
        public int NforAlert { get => _NforAlert; set { _NforAlert = value; RecalculateValues(); } }
        */
        //файл алерта
        private string _alertSound = "alert1";
        [Display(GroupName = "Оповещение", Order = 13, Name = "Звук Алерта")]
        public string alertSound { get => _alertSound; set { _alertSound = value; RecalculateValues(); } }
        #endregion

        #region Настройка. Уровни #20
        //checkbox для PitBars
        private bool _ShowPit;
        [Display(GroupName = "Уровни", Order = 20, Name = "Показывать Уровень Pit")]
        public bool ShowPit { get => _ShowPit; set { _ShowPit = value; RecalculateValues(); } }

        private Color _PitColor = Colors.Black;
        [Display(GroupName = "Уровни", Order = 21, Name = "Цвет уровня Pit")]
        public Color PitColor { get => _PitColor; set { _PitColor = value; RecalculateValues(); } }

        private VisualMode _PitType = VisualMode.Dots;
        [Display(GroupName = "Уровни", Order = 22, Name = "Форма уровня Pit")]
        public VisualMode PitType { get => _PitType; set { _PitType = value; RecalculateValues(); } }

        private int _PitSize = 1;
        [Display(GroupName = "Уровни", Order = 23, Name = "Размер формы Pit")]
        public int PitSize { get => _PitSize; set { _PitSize = value; RecalculateValues(); } }

        private ValueDataSeries _Pit = new ValueDataSeries("Уровень Pit")
        {
            IsHidden = true,
            ShowCurrentValue = false,
            ShowZeroValue = false
        };

        //checkbox для MaxV
        private bool _ShowMaxVolume;
        [Display(GroupName = "Уровни", Order = 25, Name = "Показывать Макс. Объём")]
        public bool ShowMaxVolume { get => _ShowMaxVolume; set { _ShowMaxVolume = value; RecalculateValues(); } }

        private Color _MaxVolumeColor = Colors.Black;
        [Display(GroupName = "Уровни", Order = 26, Name = "Цвет Макс. Объёма")]
        public Color MaxVolumeColor { get => _MaxVolumeColor; set { _MaxVolumeColor = value; RecalculateValues(); } }

        private VisualMode _MaxVolumeType = VisualMode.Hash;
        [Display(GroupName = "Уровни", Order = 27, Name = "Форма Макс. Объема")]
        public VisualMode MaxVolumeType { get => _MaxVolumeType; set { _MaxVolumeType = value; RecalculateValues(); } }

        private int _MaxVolumeSize = 1;
        [Display(GroupName = "Уровни", Order = 28, Name = "Размер формы Макс. Объема")]
        public int MaxVolumeSize { get => _MaxVolumeSize; set { _MaxVolumeSize = value; RecalculateValues(); } }

        private readonly ValueDataSeries _MaxBarVolume = new ValueDataSeries("Максимальный объём")
        {
            IsHidden = true,
            ShowCurrentValue = false,
            ShowZeroValue = false,
        };
        #endregion

        #region Настройка. Объём #30
        private bool _VolumeOn;
        [Display(GroupName = "Настройки Объёма", Order = 30, Name = "Учитывать Объём")]
        public bool VolumeOn { get => _VolumeOn; set { _VolumeOn = value; RecalculateValues(); } }

        private int _MinVolume = 0;
        [Display(GroupName = "Настройки Объёма", Order = 31, Name = "Объём больше или равен")]
        public int MinVolume { get => _MinVolume; set { _MinVolume = value; RecalculateValues(); } }

        private int _MaxVolume = 0;
        [Display(GroupName = "Настройки Объёма", Order = 32, Name = "Объём меньше или равен")]
        public int MaxVolume { get => _MaxVolume; set { _MaxVolume = value; RecalculateValues(); } }

        private bool _VolumeUp;
        [Display(GroupName = "Настройки Объёма", Order = 33, Name = "Растущий Объём")]
        public bool VolumeUp { get => _VolumeUp; set { _VolumeUp = value; RecalculateValues(); } }
        #endregion

        #region Настройка. Дельта #40
        //checkbox для Дельты
        private bool _DeltaOn;
        [Display(GroupName = "Настройки Дельты", Order = 40, Name = "Учитывать Дельту")]
        public bool DeltaOn { get => _DeltaOn; set { _DeltaOn = value; RecalculateValues(); } }

        private int _MinDelta = 0;
        [Display(GroupName = "Настройки Дельты", Order = 41, Name = "Дельта больше или равна")]
        public int MinDelta { get => _MinDelta; set { _MinDelta = value; RecalculateValues(); } }

        private int _MaxDelta = 0;
        [Display(GroupName = "Настройки Дельты", Order = 42, Name = "Дельта меньше или равна")]
        public int MaxDelta { get => _MaxDelta; set { _MaxDelta = value; RecalculateValues(); } }

        private bool _DeltaTailPlus;
        [Display(GroupName = "Настройки Дельты", Order = 43, Name = "Учитывать \"хвост\" Дельты", Description = "Учитывает \"хвост\" противоположного действия, которое было поглощено/продавлено")]
        public bool DeltaTailPlus { get => _DeltaTailPlus; set { _DeltaTailPlus = value; RecalculateValues(); } }
        #endregion

        #region Настройка. Отрытый Интерес #50
        //checkbox для ОИ
        private bool _OIOn;
        [Display(GroupName = "Настройки Открытого Интереса", Order = 50, Name = "Учитывать ОИ")]
        public bool OIOn { get => _OIOn; set { _OIOn = value; RecalculateValues(); } }

        private int _MinOI = 0;
        [Display(GroupName = "Настройки Открытого Интереса", Order = 51, Name = "ОИ больше или равно", Description = "Здесь под ОИ понимается изменение ОИ относительно предыдущего бара")]
        public int MinOI { get => _MinOI; set { _MinOI = value; RecalculateValues(); } }

        private int _MaxOI = 0;
        [Display(GroupName = "Настройки Открытого Интереса", Order = 52, Name = "ОИ меньше или равно", Description = "Здесь под ОИ понимается изменение ОИ относительно предыдущего бара")]
        public int MaxOI { get => _MaxOI; set { _MaxOI = value; RecalculateValues(); } }

        private bool _OITailPlus;
        [Display(GroupName = "Настройки Открытого Интереса", Order = 53, Name = "Учитывать \"хвост\" ОИ", Description = "Учитывает противоположный \"хвост\", если смотреть в режиме свечи")]
        public bool OITailPlus { get => _OITailPlus; set { _OITailPlus = value; RecalculateValues(); } }
        #endregion

        #region Настройка. Параметры бара #60
        private bool _BarSetOn;
        [Display(GroupName = "Настройки Бара", Order = 60, Name = "Учитывать настройки Бара")]
        public bool BarSetOn { get => _BarSetOn; set { _BarSetOn = value; RecalculateValues(); } }

        private decimal _MaxBarSize = 0;
        [Display(GroupName = "Настройки Бара", Order = 61, Name = "Макс. высота Бара", Description = "Если есть гэп, то учитывается по закрытию предыдущего бара.")]
        public decimal MaxBarSize { get => _MaxBarSize; set { _MaxBarSize = value; RecalculateValues(); } }

        private decimal _MinBarSize = 0;
        [Display(GroupName = "Настройки Бара", Order = 62, Name = "Мин. высота Бара", Description = "Если есть гэп, то учитывается по закрытию предыдущего бара")]
        public decimal MinBarSize { get => _MinBarSize; set { _MinBarSize = value; RecalculateValues(); } }

        private bool _UpBar = true;
        [Display(GroupName = "Настройки Бара", Order = 63, Name = "Up Бар", Description = "Считается таким, если закрытие текущего бара выше закрытия предыдущего")]
        public bool UpBar { get => _UpBar; set { _UpBar = value; RecalculateValues(); } }

        private bool _DownBar = true;
        [Display(GroupName = "Настройки Бара", Order = 64, Name = "Down Бар", Description = "Считается таким, если закрытие текущего бара ниже закрытия предыдущего")]
        public bool DownBar { get => _DownBar; set { _DownBar = value; RecalculateValues(); } }
        #endregion

        #region Настройка. Разметка бара #70
        private Color _BarColor = Colors.Transparent;
        [Display(GroupName = "Разметка бара", Order = 70, Name = "Цвет бара")]
        public Color BarColor { get => _BarColor; set { _BarColor = value; RecalculateValues(); } }

        //private VisualMode _MarkType = VisualMode.Hide;
        //[Display(GroupName = "Разметка бара", Order = 71, Name = "Форма Метки")]
        //public VisualMode MarkType { get => _MarkType; set { _MarkType = value; RecalculateValues(); } }

        //private int _MarkSize = 1;
        //[Display(GroupName = "Разметка бара", Order = 72, Name = "Размер Метки")]
        //public int MarkSize { get => _MarkSize; set { _MarkSize = value; RecalculateValues(); } }

        //private Color _MarkColor = Colors.Transparent;
        //[Display(GroupName = "Разметка бара", Order = 73, Name = "Цвет Метки")]
        //public Color MarkColor { get => _MarkColor; set { _MarkColor = value; RecalculateValues(); } }

        //private bool _MarkUnderBar = false;
        //[Display(GroupName = "Разметка бара", Order = 74, Name = "Метка под баром")]
        //public bool MarkUnderBar { get => _MarkUnderBar; set { _MarkUnderBar = value; RecalculateValues(); } }

        //private int _MarkDistance = 0;
        //[Display(GroupName = "Разметка бара", Order = 75, Name = "Смещение Метки", Description = "Смещение метки относительно экстремума бара, где 1 = 1 шагу цены актива (Пример: для RTS 1 = 10 пп)")]
        //public int MarkDistance { get => _MarkDistance; set { _MarkDistance = value; RecalculateValues(); } }

        private ValueDataSeries _BarMark = new ValueDataSeries("Метка бара")
        {
            IsHidden = true,
            //Color = Colors.Transparent,
            //VisualType = VisualMode.Hide,
            //Width = 1,
            ShowCurrentValue = false,
            ShowZeroValue = false
        };

        private readonly PaintbarsDataSeries _paintBars = new PaintbarsDataSeries("Закраска бара")
        {
            IsHidden = true,
            Visible = true,
            DrawAbovePrice = true,
        };
        #endregion

        #region Настройка. Переменные
        private decimal barHigh;
        private decimal barLow;
        private decimal barSize;

        private int _lastAlertBar = -1;

        private bool Profi;
        #endregion

        #region Настройка. Насыщенность
        private bool _isSaturation;
        [Display(GroupName = "Насыщенность", Order = 0, Name = "Вкл. Индикатор Насыщенность")]
        public bool isSaturation { get => _isSaturation; set { _isSaturation = value; RecalculateValues(); } }

        public decimal _SaturationPercentage = 8;
        [Display(GroupName = "Насыщенность", Order = 0, Name = "Процент насыщенности")]
        public decimal SaturationPercentage { get => _SaturationPercentage; set { _SaturationPercentage = value; RecalculateValues(); } }

        private Color _PositiveDeltaColor = Colors.Blue;
        [Display(GroupName = "Насыщенность", Order = 0, Name = "Цвет положительной дельты")]
        public Color PositiveDeltaColor { get => _PositiveDeltaColor; set { _PositiveDeltaColor = value; RecalculateValues(); } }

        private Color _NegativeDeltaColor = Colors.Pink;
        [Display(GroupName = "Насыщенность", Order = 0, Name = "Цвет отрицательной дельты")]
        public Color NegativeDeltaColor { get => _NegativeDeltaColor; set { _NegativeDeltaColor = value; RecalculateValues(); } }

        // Настройки меток баров для насыщенности
        private bool _isSaturationBarMarks;
        [Display(GroupName = "Насыщенность", Order = 0, Name = "Вкл. метки баров")]
        public bool isSaturationBarMarks { get => _isSaturationBarMarks; set { _isSaturationBarMarks = value; RecalculateValues(); } }

        private VisualMode _SaturationMarkType = VisualMode.Hide;
        [Display(GroupName = "Насыщенность", Order = 71, Name = "Форма Метки")]
        public VisualMode SaturationMarkType { get => _SaturationMarkType; set { _SaturationMarkType = value; RecalculateValues(); } }

        private int _SaturationMarkSize = 1;
        [Display(GroupName = "Насыщенность", Order = 72, Name = "Размер Метки")]
        public int SaturationMarkSize { get => _SaturationMarkSize; set { _SaturationMarkSize = value; RecalculateValues(); } }

        private Color _SaturationMarkColor = Colors.Transparent;
        [Display(GroupName = "Насыщенность", Order = 73, Name = "Цвет Метки")]
        public Color SaturationMarkColor { get => _SaturationMarkColor; set { _SaturationMarkColor = value; RecalculateValues(); } }

        private bool _SaturationMarkUnderBar = false;
        [Display(GroupName = "Насыщенность", Order = 74, Name = "Метка под баром")]
        public bool SaturationMarkUnderBar { get => _SaturationMarkUnderBar; set { _SaturationMarkUnderBar = value; RecalculateValues(); } }

        private int _SaturationMarkDistance = 0;
        [Display(GroupName = "Насыщенность", Order = 75, Name = "Смещение Метки", Description = "Смещение метки относительно экстремума бара, где 1 = 1 шагу цены актива (Пример: для RTS 1 = 10 пп)")]
        public int SaturationMarkDistance { get => _SaturationMarkDistance; set { _SaturationMarkDistance = value; RecalculateValues(); } }

        #endregion

        #region Настройка. Насыщенность по Открытому интересу
        private bool _isOiSaturation;
        [Display(GroupName = "Насыщенность по ОИ", Order = 0, Name = "Вкл. Индикатор ОИ Насыщенность")]
        public bool isOiSaturation { get => _isOiSaturation; set { _isOiSaturation = value; RecalculateValues(); } }

        public decimal _oiSaturationPercentage = 8;
        [Display(GroupName = "Насыщенность по ОИ", Order = 0, Name = "Процент насыщенности")]
        public decimal oiSaturationPercentage { get => _oiSaturationPercentage; set { _oiSaturationPercentage = value; RecalculateValues(); } }

        private Color _oiPositiveColor = Colors.Blue;
        [Display(GroupName = "Насыщенность по ОИ", Order = 0, Name = "Цвет положительной дельты")]
        public Color oiPositiveColor { get => _oiPositiveColor; set { _oiPositiveColor = value; RecalculateValues(); } }

        private Color _oiNegativeColor = Colors.Pink;
        [Display(GroupName = "Насыщенность по ОИ", Order = 0, Name = "Цвет отрицательной дельты")]
        public Color oiNegativeColor { get => _oiNegativeColor; set { _oiNegativeColor = value; RecalculateValues(); } }

        // Настройки меток баров для насыщенности
        private bool _isOiSaturationBarMarks;
        [Display(GroupName = "Насыщенность по ОИ", Order = 0, Name = "Вкл. метки баров")]
        public bool isOiSaturationBarMarks { get => _isOiSaturationBarMarks; set { _isOiSaturationBarMarks = value; RecalculateValues(); } }

        private VisualMode _oiSaturationMarkType = VisualMode.Hide;
        [Display(GroupName = "Насыщенность по ОИ", Order = 71, Name = "Форма Метки")]
        public VisualMode oiSaturationMarkType { get => _oiSaturationMarkType; set { _oiSaturationMarkType = value; RecalculateValues(); } }

        private int _oiSaturationMarkSize = 1;
        [Display(GroupName = "Насыщенность по ОИ", Order = 72, Name = "Размер Метки")]
        public int oiSaturationMarkSize { get => _oiSaturationMarkSize; set { _oiSaturationMarkSize = value; RecalculateValues(); } }

        private Color _oiSaturationMarkColor = Colors.Transparent;
        [Display(GroupName = "Насыщенность по ОИ", Order = 73, Name = "Цвет Метки")]
        public Color oiSaturationMarkColor { get => _oiSaturationMarkColor; set { _oiSaturationMarkColor = value; RecalculateValues(); } }

        private bool _oiSaturationMarkUnderBar = false;
        [Display(GroupName = "Насыщенность по ОИ", Order = 74, Name = "Метка под баром")]
        public bool oiSaturationMarkUnderBar { get => _oiSaturationMarkUnderBar; set { _oiSaturationMarkUnderBar = value; RecalculateValues(); } }

        private int _oiSaturationMarkDistance = 0;
        [Display(GroupName = "Насыщенность по ОИ", Order = 75, Name = "Смещение Метки", Description = "Смещение метки относительно экстремума бара, где 1 = 1 шагу цены актива (Пример: для RTS 1 = 10 пп)")]
        public int oiSaturationMarkDistance { get => _oiSaturationMarkDistance; set { _oiSaturationMarkDistance = value; RecalculateValues(); } }

        #endregion

        #region Индикатор. ПУБЛИКАЦИЯ
        public VDOWithSaturation() : base(true)
        {
            ValueDataSeries main = (ValueDataSeries)DataSeries[0];
            main.IsHidden = true;

            Panel = IndicatorDataProvider.CandlesPanel;
            DenyToChangePanel = true;

            DataSeries.Add(_BarMark);
            DataSeries.Add(_paintBars);
            DataSeries.Add(_MaxBarVolume);
            DataSeries.Add(_Pit);
        }
        #endregion

        #region [ОСНОВНОЙ БЛОК] Индикатор. РАСЧЁТ
        protected override void OnCalculate(int bar, decimal value)
        {
            //this[bar] = 0;
            if (bar < 2 || !OnOff) return;
            _paintBars[bar] = null;
            _BarMark[bar] = _MaxBarVolume[bar] = _Pit[bar] = 0;

            if (UseAlert) IfProfiSendAlert(bar, NameConstructor);

            GetBarProfi(bar);

            if (Profi)
            {
                //уровни пит, макс.объем
                if (ShowPit) _Pit[bar] = GetBarPit(bar);
                    _Pit.Color = PitColor;
                    _Pit.VisualType = PitType;
                    _Pit.Width = PitSize;
                if (ShowMaxVolume) _MaxBarVolume[bar] = GetCandle(bar).MaxVolumePriceInfo.Price;
                    _MaxBarVolume.Color = MaxVolumeColor;
                    _MaxBarVolume.VisualType = MaxVolumeType;
                    _MaxBarVolume.Width = MaxVolumeSize;

                //метка и раскраска
                //_BarMark[bar] = barHigh + (MarkDistance * InstrumentInfo.TickSize);
                //if (MarkUnderBar) _BarMark[bar] = barLow - (MarkDistance * InstrumentInfo.TickSize);
                //    if (BarColor != Colors.Transparent) _paintBars[bar] = BarColor;
                //    _BarMark.Color = MarkColor;
                //    _BarMark.VisualType = MarkType;
                //    _BarMark.Width = MarkSize;


                // Расчет насыщенности
                if (_isSaturation)
                {
                    var delta = GetCandle(bar).Delta;
                    var volume = GetCandle(bar).Volume;

                    if (volume == 0)
                        return;

                    var saturation = Math.Abs(delta / volume) * 100;

                    if (saturation >= _SaturationPercentage )
                    {
                        _paintBars[bar] = delta > 0 ? _PositiveDeltaColor : _NegativeDeltaColor;

                        //метка насыщенности
                        //if (_isSaturationBarMarks) {
                            _BarMark[bar] = barHigh + (SaturationMarkDistance * InstrumentInfo.TickSize);
                            if (SaturationMarkUnderBar) _BarMark[bar] = barLow - (SaturationMarkDistance * InstrumentInfo.TickSize);
                            _BarMark.Color = SaturationMarkColor;
                            _BarMark.VisualType = SaturationMarkType;
                            _BarMark.Width = SaturationMarkSize;
                        //}
                    }
                }

                // Расчет насыщенности по ОИ
                if (_isOiSaturation)
                {
                    var oi = GetCandle(bar).OI;
                    var volume = GetCandle(bar).Volume;

                    if (volume == 0)
                        return;

                    var oiSaturation = Math.Abs(oi / volume) * 100;

                    if (oiSaturation >= _oiSaturationPercentage)
                    {
                        _paintBars[bar] = oi > 0 ? _oiPositiveColor : _oiNegativeColor;

                        //метка насыщенности по ОИ
                        //if (_isSaturationBarMarks) {
                        _BarMark[bar] = barHigh + (oiSaturationMarkDistance * InstrumentInfo.TickSize);
                        if (oiSaturationMarkUnderBar) _BarMark[bar] = barLow - (oiSaturationMarkDistance * InstrumentInfo.TickSize);
                        _BarMark.Color = oiSaturationMarkColor;
                        _BarMark.VisualType = oiSaturationMarkType;
                        _BarMark.Width = oiSaturationMarkSize;
                        //}
                    }
                }


            }
        }
        #endregion

        //--------------------------------- МЕТОДЫ --------------------------------

        #region Расчёт Минимума и Максимума Бара (с учётом возможного гепа)
        private decimal GetBarHigh(int bar)
        {
            IndicatorCandle Candle = GetCandle(bar);
            IndicatorCandle preCandle = GetCandle(bar - 1);
            decimal barClose = Candle.Close;
            decimal barHigh = Candle.High;
            decimal preBarClose = preCandle.Close;

            //визуально продавец
            if (barClose < preBarClose)
            {
                //есть геп
                if (preBarClose > barHigh) barHigh = preBarClose;
            }
            //визуально нейтраль
            else if (preBarClose == barClose)
            {
                //есть геп
                if (preBarClose > barHigh) barHigh = preBarClose;
            }

            return barHigh;
        }

        private decimal GetBarLow(int bar)
        {
            IndicatorCandle Candle = GetCandle(bar);
            IndicatorCandle preCandle = GetCandle(bar - 1);
            decimal barClose = Candle.Close;
            decimal barLow = Candle.Low;
            decimal preBarClose = preCandle.Close;

            //визуально покупатель
            if (barClose > preBarClose)
            {
                //есть геп
                if (preBarClose < barLow) barLow = preBarClose;
            }
            //визально нейтраль
            else if (preBarClose == barClose)
            {
                //есть геп
                if (preBarClose < barLow) barLow = preBarClose;
            }

            return barLow;
        }
        #endregion

        #region Проверка Объема
        private bool CheckBarVolume(int bar)
        {
            if (!VolumeOn) return true;

            decimal barVolume = GetCandle(bar).Volume;
            decimal preBarVolume = GetCandle(bar - 1).Volume;
            bool minVolume = true;
            bool maxVolume = true;
            bool upVolume = true;

            if (MinVolume != 0 && barVolume < MinVolume) minVolume = false;
            if (MaxVolume != 0 && barVolume > MaxVolume) maxVolume = false;
            if (VolumeUp && barVolume - preBarVolume < 0) upVolume = false;

            if (minVolume && maxVolume && upVolume) return true;
            else return false;
        }
        #endregion

        #region Проверка Дельты
        private bool CheckBarDelta(int bar)
        {
            if (!DeltaOn) return true;

            decimal barDelta = GetCandle(bar).Delta;
            decimal barMaxDelta = GetCandle(bar).MaxDelta;
            decimal barMinDelta = GetCandle(bar).MinDelta;

            bool minDelta = true;
            bool maxDelta = true;

            if (DeltaTailPlus)
            {
                if (barDelta >= 0 && barMinDelta < 0) { barDelta -= barMinDelta;}
                if (barDelta < 0 && barMaxDelta > 0) { barDelta -= barMaxDelta;}
            }

            if (MinDelta != 0 && barDelta < MinDelta) minDelta = false;
            if (MaxDelta != 0 && barDelta > MaxDelta) maxDelta = false;

            if (minDelta && maxDelta) return true;
            else return false;
        }
        #endregion

        #region Проверка ОИ
        private bool CheckBarOI(int bar)
        {
            if (!OIOn) return true;

            decimal barOI = GetCandle(bar).OI - GetCandle(bar - 1).OI;
            decimal barMaxOI = GetCandle(bar).MaxOI - GetCandle(bar - 1).OI;
            decimal barMinOI = GetCandle(bar).MinOI - GetCandle(bar - 1).OI;

            bool minOI = true;
            bool maxOI = true;

            if (OITailPlus)
            {
                if (barOI >= 0 && barMinOI < 0) { barOI -= barMinOI; }
                if (barOI < 0 && barMaxOI > 0) { barOI -= barMaxOI; }
            }

            if (MinOI != 0 && barOI < MinOI) minOI = false;
            if (MaxOI != 0 && barOI > MaxOI) maxOI = false;

            if (minOI && maxOI) return true;
            else return false;
        }
        #endregion

        #region Проверка Параметров Бара
        private bool CheckBarSet(int bar)
        {
            if (!BarSetOn) return true;

            decimal barSize = GetBarHigh(bar) - GetBarLow(bar);
            decimal barTrend = GetCandle(bar).Close - GetCandle(bar - 1).Close;
            bool upBar = true;
            bool downBar = true;
            bool minSize = true;
            bool maxSize = true;

            if (MinBarSize > 0 && barSize < MinBarSize) minSize = false;
            if (MaxBarSize > 0 && barSize > MaxBarSize) maxSize = false;
            if (UpBar && barTrend <= 0) upBar = false;
            if (DownBar && barTrend >= 0) downBar = false;
            if (UpBar && DownBar) downBar = upBar = true;
            if (!UpBar && !DownBar && barTrend != 0) downBar = upBar = false;

            if (minSize && maxSize && upBar && downBar) return true;
            else return false;
        }
        #endregion

        #region Проверка Profi
        private void GetBarProfi(int bar)
        {
            barHigh= GetBarHigh(bar);
            barLow= GetBarLow(bar);
            barSize = barHigh - barLow;

            if (CheckBarDelta(bar) && CheckBarVolume(bar) && CheckBarOI(bar) && CheckBarSet(bar)) Profi = true;
            else Profi = false;
        }
        #endregion

        #region Расчёт PIT
        private decimal GetBarPit(int bar)
        {
            IndicatorCandle candle = GetCandle(bar);
            decimal pitbar = candle.High;

            if (candle.High != candle.Low)
            {
                pitbar = Math.Round((candle.Low + Math.Abs(candle.Volume - candle.Delta) / 2 / candle.Volume * (candle.High - candle.Low)) / InstrumentInfo.TickSize) * InstrumentInfo.TickSize;
            }

            return pitbar;
        }
        #endregion

        #region Алерт
        private void IfProfiSendAlert(int bar, string NameConstructior)
        {
            int workBar = bar - 1;
            if (bar == _lastAlertBar || bar < CurrentBar - 1) return;
            string barTime = GetCandle(workBar).Time.ToString("HH:mm");

            GetBarProfi(workBar);

            if (Profi)
            {
                AddAlert(alertSound,
                    $"{Environment.NewLine}{NameConstructor}" +
                    $"{Environment.NewLine}🕖{barTime}, ↕{barSize}" +
                    $"{Environment.NewLine}🔼{barHigh}" +
                    $"{Environment.NewLine}🔽{barLow}"
                    );
                _lastAlertBar = bar;
            }

            Profi = false;

            return;
        }
        #endregion
    }
}

