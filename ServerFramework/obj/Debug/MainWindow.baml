   M S B A M L   `   `   `  ����J  FServerFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ServerFrameworkServerFramework  5   	        ServerFramework.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:ServerFramework     5   	     � Title$   Server��5   	   $	��550��6   $	��525��6&   .��5	      �  � RowDefinitions 5
   
   �� 5      $��50 �6   6*   �� 5      $	��368 �6   6+   �� 5      $
��99.2 �6   6,   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��0,0,0.4,0.4q�6;   .+�5      �� -   5        $��lbMsg��5      $��状态信息：��6!   $��10,10q�61   6B   5      �� 5   
   -   5        $��txtReceiveMsg��5      $��1��6   $��10,9.6,10.4,7.2q�64   
��� d � �� TextWrapping$
 Wrap=�6e   " �� VerticalScrollBarVisibility$
 Auto=�6y   
����  � �� 
IsReadOnly . 6�   �� 5   
   $��2��6   $ 
Horizontal=�6"   $��0,0,0.4,0.4q�6;   .+�5      �� -   5        $��
btnConnect��5      $��启动��6'   $	��100��64   $��30��6@   $
��15,8q�6L   6u   �� 5      -   5        $��btnDisConnect��5      $��关闭��6*   $	��100��67   $��30��6C   $
��15,8q�6O   6{   �� 5      -   5        $��LeapTest��5      $��
Leap测试��6%   $	��100��66   $��30��6B   $
��15,8q�6N   6x   5      5      5      