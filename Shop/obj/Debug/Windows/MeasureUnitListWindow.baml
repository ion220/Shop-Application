   M S B A M L   `   `   `  ?????  ;Shop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Shop.WindowsShop.Windows  5   	   (    "Shop.Windows.MeasureUnitListWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Shop.Windows     5   	     ? Title$6  0Просмотр едениц измерения??5   	   $	??400??6+   $	??350??68    ? 
ResizeMode$ NoResize=?6D   .??5	      ?  ? RowDefinitions 5
   
   ?? 5      $
??1.2* ?6   6,   ?? 5      $??6* ?6   6*   ?? 5      $??* ?6   6)   ?? 5      $
??2.5* ?6   6,    ? ColumnDefinitions 5   
   ?? 5      $??* ?6   6+   ?? 5      $??* ?6   6+   ?? 5      $??* ?6   6+   .+?5   
   ?? $??3??6   $??Center=?6(   $??Center=?6E   $??16(?6`   #!Еденицы измерения6?   &   System.Windows.Controls.DataGrid  5   
   -   5        $??UnitsDataGrid??5      $??3??6(   $??1??6<      AutoGenerateColumns .  $??Center=?6e      
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID??6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 61   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6;   ?? *id+6T     5      $ Название??6%   $ 5* 67   	 6B   ?? *name+6]     5      $% Краткое название??6%   $ 3* 6?   	 6J   ?? *
short_name+6k   5      ?? 5   
   -   5        $??
EditButton??5      $??2??6#   $??0??60   $??Center=?6@   $??90??6]   $??Center=?6h   .??6?   Редактировать6?   ?? 5   
   -   5        $??CreateButton??5      $??2??6%   $??1??62   $??Center=?6B   $??90??6_   $??Center=?6j   .??6?   Создать6?   ?? 5    
   -   5        $??DeleteButton??5       $??2??6%   $??2??62   $??Center=?6B   $??90??6_   $??Center=?6j   .??6?   Удалить6?   ?? 5"   
   $??3??6   $??3??6   
???????$??1q?6E   $??3q?6Y   ._?5#      ? -   5        $??EditToolsGrid??5#      {?.   5$      ?? 5%      $??* ?6$   61   ?? 5&      $??* ?6$   61   ?? 5'      $??* ?6$   61    5)      ?? 5*      $??* ?6'   63   ?? 5+      $??5* ?6'   64   ?? 5,      $??3* ?6'   64   .+?5/      ?? $??0??6   $??Bottom=?6,   $??5,0,5,0q?6G   ID:6^   ?? 50      $??1??6   $??Bottom=?6,   $??5,0,5,0q?6G   Название:6d   ?? 51      $??2??6   $??Bottom=?6,   $??5,0,5,0q?6G   " Краткое название:6l   ?? 53      -   5        $??IdTextBlock??53      $??1??6/   $??Center=?6<   $??5q?6W   Auto6i   ?? 54      -   5        $??NameTextBox??54      $??1??6-   $??1??6:   $??Center=?6J   $??5,0,5,0q?6e   
 ?? 	MaxLength$
 50??6v   6?   ?? 55      -   5        $??ShortNameTextBox??55      $??1??62   $??2??6?   $??Center=?6O   $??5,0,5,0q?6j   $
 10??6{   6?   ?? 57      -	   5        $??
SaveButton??57   :   $??3??6   $??3??6&   $??Center=?6L   $??Center=?6i   .??6?   Сохранить6?   58      59      5:      5;      