   M S B A M L   `   `   `  ?????  ;Shop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Shop.WindowsShop.Windows  5   	   #    Shop.Windows.GroupsListWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Shop.Windows     5   	     ? Title$!  Группы товаров??5   	   $	??600??6    $	??500??6-    ? 
ResizeMode$ NoResize=?69   .??5	      ?  ? RowDefinitions 5
   
   ?? 5      $??* ?6   6)   ?? 5      $??5* ?6   6*   ?? 5      $
??0.4* ?6   6,   ?? 5      $??* ?6   6)    ? ColumnDefinitions 5   
   ?? 5      $??* ?6   6+   ?? 5      $??* ?6   6+   ?? 5      $??2* ?6   6,   .+?5   
   ?? $??3??6   $??Center=?6(   $??Center=?6E   $??16(?6`   Группы товаров6~   R? 5   
   -   5        $??GroupsTreeView??5      $??1??6)   $??3??66   $??5q?6J   ??5      ?? 5       ?? ItemsSource 6+   ??  ?? Path$ Groups?.R?5      ?? ??6    ?? *Name+5      5      ?? 5   
   -   5        $??ExpandButton??5      $??2??6%   $??2??62   $	??190??6F   $??Center=?6R   $??Center=?6o   .??6?   20Развернуть главные группы6?   ?? 5!   
   -   5        $??
EditButton??5!      $??3??6#   $??0??60   $??Center=?6@   $??Center=?6]   $	??100??6x   .??6?   Редактировать6?   ?? 5"   
   -   5        $??DeleteButton??5"      $??3??6%   $??1??62   $??Center=?6B   $??Center=?6_   $	??100??6z   .??6?   Удалить6?   ?? 5#   
   -   5        $??CreateButton??5#      $??3??6%   $??2??62   $??Center=?6B   $??Center=?6_   $	??100??6z   .??6?   Создать6?   5$      5%      