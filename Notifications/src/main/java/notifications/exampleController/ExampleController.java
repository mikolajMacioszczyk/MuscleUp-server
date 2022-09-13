package notifications.exampleController;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("test")
public class ExampleController {

    @GetMapping("/notificationsExample")
    String notificationsExample() {

        return "Notifications endpoint works fine";
    }
}
