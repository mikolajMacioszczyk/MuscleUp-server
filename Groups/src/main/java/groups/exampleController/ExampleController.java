package groups.exampleController;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("test")
public class ExampleController {

    @GetMapping("/groupsExample")
    String groupsExample() {

        return "Groups endpoint works fine";
    }
}
