   M S B A M L   `   `   `  ?????  ;Shop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Shop.WindowsShop.Windows  5   	   "    Shop.Windows.EditGroupWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Shop.Windows     5   	     ? Title$D  >Редактирование/добавление группы??5   	   $	??350??62   $	??500??6?    ? 
ResizeMode$ NoResize=?6K   .??5	      ?  ? RowDefinitions 5
   
   ?? 5      $??* ?6   6)   ?? 5      $??* ?6   6)   ?? 5      $??* ?6   6)   ?? 5      $??* ?6   6)   ?? 5      $??5* ?6   6*   ?? 5      $??* ?6   6)    ? ColumnDefinitions 5   
   ?? 5      $??2* ?6   6,   ?? 5      $??5* ?6   6,   .+?5   
   ?? -   5        $??	TitleText??5      $??2??6%   $??Center=?69   $??Center=?6V   $??16(?6q   #!Добавление группы6?   ?? 5   
   $??1??6   $??Right=?6!   $??Center=?6=   $??5q?6X   Название:6n   ?? 5   
   $??2??6   $??Right=?6!   $??Center=?6=   $??5q?6X   (&Является подгруппой:6y   ?? 5   
   -   5        $??TreeViewTitleTextBlock??5      $??3??62   $??Center=?6?   $??Center=?6\   -+Выберите мастер-группу:6?   ?? 5   
   -   5        $??NameTextBox??5      $??1??6%   $??1??62   $??Center=?6B   $
??Left=?6]   $	??200??6x    ?? 	MaxLength$ 50??6?   6?   ?? 5   
   -   5        $??HasParentGroupCheckBox??5      $??1??61   $??2??6A   $
??Left=?6N   $??Center=?6i   6?   R? 5   
   -   5        $??GroupsTreeView??5      $??3??6)   $??2??66   $??1??6G   $??5q?6W   ??5       ?? 5!       ?? ItemsSource 6+   ??  ?? Path$ Groups?.R?5"      ?? ??6    ?? *Name+5#      5%      ?? 5'   
   -   5        $??
SaveButton??5'   2   $??5??6   $??2??6   $??Center=?6D   $??Center=?6a   .??6?   Сохранить6?   5(      5)      