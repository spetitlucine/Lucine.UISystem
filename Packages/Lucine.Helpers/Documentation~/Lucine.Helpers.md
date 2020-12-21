# Lucine.Helpers

Some little tools that help Lucine Apps

[Doxygen documentation is here](html/index.html)

## Lucine.Helpers.Events

A simple Event System for Lucine Applications

### The Event System {#the-event-system .Titre1}

The event system is a tiny tools to help sending and receiving notifications of events.

You define an eventType inherited from Event (event with no parameter)

```c#
public class ApplicationQuitEvent : Event { }
```

Or from the templated version which accept one parameter

```c#
public class OnPanelClosedEvent : Event<DemoPanel> { }
```

Then elsewhere you Add a listener to the defined type and set a function that will be called when the event will be fired. The function should have no parameter or the templated parameter type

```c#
Events.Instance.TypeOf<DemoPanel.OnPanelClosedEvent>().AddListener(OnPanel1Closed);
```

When no more need of the notification you can remove the listener

```c#
Events.Instance.TypeOf<DemoPanel.OnPanelClosedEvent>().RemoveListener(OnPanel1Closed);
```

When you want to fire the event you dispatch it !

```c#
Events.Instance.TypeOf<OnPanelClosedEvent>().Dispatch(this);
```

The Events class is a singleton that let you have a global pool of events. But you also can have pools dedicated to special features if you want. It this case you have to define your own pool using the class EventPool (that's what Events do)

That's so easy so use it !

## Lucine.Helpers.TextManager {#lucine.helpers.textmanager .Titre1}

The role of the text manager is to keep trace of all text of the application.

All texts are defined in an xml file using Ids and Text with the following format

```xml
<?xml version="1.0" encoding="utf-16"?>
<TextDatabase xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Texts>
    <Entry Id="ID_SHOWPANEL1" Text="Show panel 1" />
    <Entry Id="ID_SHOWPANEL2" Text="Show panel 2" />
    <Entry Id="ID_TITLEPANEL1" Text="Panel 1" />
    <Entry Id="ID_TITLEPANEL2" Text="Panel 2" />
    <Entry Id="ID_CLOSEPANEL1" Text="Close Panel 1" />
    <Entry Id="ID_CLOSEPANEL2" Text="Close Panel 2" />
    <Entry Id="ID_QUIT" Text="Quit" />
    <Entry Id="ID_QUITMSG" Text="Ok to quit ?" />
    <Entry Id="ID_OK" Text="OK" />
    <Entry Id="ID_CANCEL" Text="Cancel" />
	<Entry Id="ID_ENTERTEXT" Text="type in text..." />
  </Texts>
</TextDatabase>
```

This xmlfile can loaded by the TextManager from a file in StreamingAssets folder, or from a Text resource in a Resources directory (in which case the extension of the file has to be .txt even if xml)

TextManager is a singleton that can be place where you want. In the sample it is located on the Starter GameObject.

![](C:\lucine\UnityProjects\UISystem\Lucine.UISystem\Packages\Lucine.Helpers\Documentation~\images\Image2.png)

You can select the TextDatabase name (no extension has to be given when using resources) and where to load it from. Resources =\> Resources folder, StreamingAssets, streaming assets folder (in this case you have to include the extension of the file). In the sample the file is present both in streamingassets and resources, in order you can test both methods.

When the file is loaded it fires an OnTextDabaseChanged event, you can register to this event to be notified when something changed in the texts.

That's what does UIText component

![](C:\lucine\UnityProjects\UISystem\Lucine.UISystem\Packages\Lucine.Helpers\Documentation~\images\Image3.png)

This component requires that the gameobject on which it is, has also a TextComponent.

The only information to set on it is the id of the text in the textdatabase that need to be set in the text component.

On startup the TextComponent is cached and UIText register itself to the OnTextDataBaseChanged event. That way when the text is ready, it is alerted and it can just ask to the TextManager the text that correspond to the id it has and set it in the text component.

```c#
/// <summary>
/// Change the text when event fired
/// </summary>
public void OnTextChanged()
{
    m_Text.text = TextManager.Instance.GetText(m_TextId);
}

```

The TextManager is useful to avoid having text in the application. All texts are outside the application.

This way translation are made easier.
