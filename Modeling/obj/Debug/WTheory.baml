   M S B A M L   `   `   `  ����C  ?Modeling, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ModelingModeling  5   	       Modeling.WTheory   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Modeling     5   	   -   5          �Name$  window��5   	    � Title$ Теорія��5   	      WindowStyle#��[  6    � WindowStartupLocation$ CenterScreen=�6=   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��auto �6   6,   �� 5      6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   6.   �� 5      6!   .+�5   
   ��   ImageBackStyle#��[ 6   $"��Sources/Images/BackEnter.png��68   6`   �� 5   
   -   5        $��im_Back��5      $��0��6   $��0��6,   #��[ 6<   6�   �� 5   
   -   5        $��lb_Menu��5      $��18(�6M   $��1��6[   $��
10,0,10,10q�6h   ��6|   �� *
Background+ �� ElementName$ window����6�   Z� 6�   (  "System.Windows.Controls.WebBrowser  5   
   -   5        $��
webBrowser��5      $��1��6'   $��1��64   $��
10,0,10,10q�6D   6Z   5      5      